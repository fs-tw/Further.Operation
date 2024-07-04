using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;

namespace Further.Abp.Operation
{
    public class OperationProvider : IOperationProvider, ISingletonDependency
    {

        protected readonly TimeSpan DefaultExpiry = TimeSpan.FromSeconds(5);

        protected readonly TimeSpan DefaultWait = TimeSpan.FromSeconds(2);

        protected readonly TimeSpan DefaultRetry = TimeSpan.FromMilliseconds(200);

        protected readonly OperationOptions options;
        protected readonly ILogger<OperationProvider> logger;
        protected readonly AbpDistributedCacheOptions distributedCacheOptions;
        protected readonly IDistributedCache<OperationInfo> distributedCache;
        protected readonly IConfiguration configuration;
        protected readonly IDistributedEventBus distributedEventBus;
        protected readonly AsyncLocal<Guid?> OperationId;

        protected RedLockFactory? redLockFactory;
        protected IDatabase? db;
        protected ConnectionMultiplexer? connection;

        public OperationProvider(
            ILogger<OperationProvider> logger,
            IOptions<OperationOptions> options,
            IOptions<AbpDistributedCacheOptions> distributedCacheOptions,
            IDistributedCache<OperationInfo> distributedCache,
            IConfiguration configuration,
            IDistributedEventBus distributedEventBus)
        {
            this.options = options.Value;
            this.logger = logger;
            this.distributedCacheOptions = distributedCacheOptions.Value;
            this.distributedCache = distributedCache;
            this.configuration = configuration;
            this.distributedEventBus = distributedEventBus;
            OperationId = new AsyncLocal<Guid?>();
        }

        public virtual Guid? GetCurrentId()
        {
            return OperationId.Value;
        }

        public virtual void SetCurrentId(Guid id)
        {
            OperationId.Value = id;
        }

        public virtual async Task InitializeAsync()
        {
            var options = ConfigurationOptions.Parse(configuration["Redis:Configuration"] + ",allowAdmin=true");

            while (connection == null)
            {
                try
                {
                    var redis = await ConnectionMultiplexer.ConnectAsync(options);

                    connection = redis;
                    db = redis.GetDatabase();
                    redLockFactory = RedLockFactory.Create(new List<RedLockMultiplexer> { redis });

                    await SubscribeKeyExpiredAsync();

                    connection!.ConnectionRestored += async (sender, args) =>
                    {
                        await SubscribeKeyExpiredAsync();
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Redis connection failed. Retrying in 5 seconds...");
                    await Task.Delay(5000);
                }
            }
        }

        public virtual async Task UpdateOperationAsync(Guid id, Action<OperationInfo> action, TimeSpan? slidingExpiration = null)
        {
            var expiryTime = DefaultExpiry;
            var waitTime = DefaultWait;
            var retryTime = DefaultRetry;

            if (connection == null || !connection!.IsConnected)
            {
                logger.LogWarning("Redis connection is down, skipping lock acquisition.");
                return;
            }


            using (var redLock = await redLockFactory!.CreateLockAsync(id.ToString(), expiryTime, waitTime, retryTime))
            {
                if (redLock.IsAcquired)
                {
                    var operationInfo = await GetAsync(id);

                    action(operationInfo);

                    await SetAsync(operationInfo, slidingExpiration);
                }
                else
                {
                    logger.LogWarning($"Operation {id} is locked.");
                }
            }
        }

        public virtual async Task CreateOperationAsync(Guid id, Action<OperationInfo> action, TimeSpan? slidingExpiration = null)
        {
            var expiryTime = DefaultExpiry;
            var waitTime = DefaultWait;
            var retryTime = DefaultRetry;

            if (connection == null || !connection!.IsConnected)
            {
                logger.LogWarning("Redis connection is down, skipping lock acquisition.");
                return;
            }

            OperationId.Value = id;

            using (var redLock = await redLockFactory!.CreateLockAsync(id.ToString(), expiryTime, waitTime, retryTime))
            {
                if (redLock.IsAcquired)
                {
                    var operationInfo = new OperationInfo(id);

                    action(operationInfo);

                    await SetAsync(operationInfo, slidingExpiration ?? options.DefaultSlidingExpiration);
                }
                else
                {
                    logger.LogWarning($"Operation {id} is locked.");
                }
            }
        }

        public virtual Task<List<Guid>> ListIdsAsync()
        {
            if (connection == null || !connection!.IsConnected)
            {
                logger.LogWarning("Redis connection is down, skipping list ids.");
                return Task.FromResult(new List<Guid>());
            }

            string pattern = $"*k:{distributedCacheOptions.KeyPrefix}*"; // 定義符合特定規則的模式
            var keys = db!.Execute("KEYS", pattern); // 使用 KEYS 指令取得符合模式的 key
            var result = new List<Guid>();

            // 返回符合規則的 key
            foreach (string key in (string[])keys!)
            {
                var operationId = RedisKeyParseOperationId(key);

                if (operationId.HasValue && !result.Contains((Guid)operationId))
                {
                    result.Add(operationId.Value);
                }
            }

            return Task.FromResult(result);
        }

        public virtual async Task RemoveAsync(Guid id)
        {
            await distributedCache.RemoveAsync(OperationConsts.GetIdKey(id));
            await distributedCache.RemoveAsync(OperationConsts.GetValueKey(id));
        }

        public virtual async Task<OperationInfo?> GetAsync(Guid id)
        {
            await distributedCache.GetAsync(OperationConsts.GetIdKey(id));
            return await distributedCache.GetAsync(OperationConsts.GetValueKey(id));
        }

        protected virtual async Task SetAsync(OperationInfo operationInfo, TimeSpan? slidingExpiration = null)
        {
            if (slidingExpiration != null)
            {
                await distributedCache.SetAsync(OperationConsts.GetIdKey(operationInfo.Id), operationInfo, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = slidingExpiration
                });

                await distributedCache.SetAsync(OperationConsts.GetValueKey(operationInfo.Id), operationInfo, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = slidingExpiration + TimeSpan.FromSeconds(10)
                });
            }

            if (slidingExpiration == null)
            {
                await distributedCache.SetAsync(OperationConsts.GetIdKey(operationInfo.Id), operationInfo);
                await distributedCache.SetAsync(OperationConsts.GetValueKey(operationInfo.Id), operationInfo);
            }
        }

        public async Task SubscribeKeyExpiredAsync()
        {
            var subscriber = connection!.GetSubscriber();

            connection.GetServer(connection.GetEndPoints().Single())
                .ConfigSet("notify-keyspace-events", "KEA");

            await subscriber.SubscribeAsync("__keyevent@0__:expired", async (channel, value) =>
            {
                if (!value.ToString().Contains($"c:{typeof(OperationInfo).FullName},k")) return;

                var operationId = RedisKeyParseOperationId(value.ToString());

                if (operationId.HasValue)
                {
                    var operationInfo = await distributedCache.GetAsync(OperationConsts.GetValueKey(operationId.Value));

                    if (operationInfo == null) return;

                    await distributedCache.RemoveAsync(OperationConsts.GetValueKey(operationId.Value));

                    await distributedEventBus.PublishAsync(new OperationExpiredEto
                    {
                        OperationInfo = operationInfo
                    });
                }
            });

            connection.ConnectionFailed += async (sender, args) =>
            {
                await subscriber.UnsubscribeAsync("__keyevent@0__:expired");
            };
        }

        protected virtual Guid? RedisKeyParseOperationId(string key)
        {
            if (key.StartsWith("t:"))
            {
                int commaIndex = key.IndexOf(',');
                key = key.Substring(commaIndex + 1);
            }

            key = key.Replace($"c{typeof(OperationInfo)}:,k:{distributedCacheOptions.KeyPrefix}", "");

            string prefix = "Ids:";
            int prefixIndex = key.IndexOf(prefix);

            if (prefixIndex != -1)
            {
                int idStartIndex = prefixIndex + prefix.Length;
                int idEndIndex = key.IndexOf(':', idStartIndex);
                if (idEndIndex == -1)
                {
                    // 如果沒有找到冒號，說明 id 部分就是最後一部分
                    return Guid.Parse(key.Substring(idStartIndex));
                }
            }

            return null;
        }
    }
}
