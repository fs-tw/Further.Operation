using FluentResults;
using Further.Operation.Operations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Further.Abp.Operation
{
    public class OperationProvider : IOperationProvider, ISingletonDependency
    {
        public Guid? CurrentId => OperationId.Value;

        private readonly RedLockFactory redLockFactory;
        private readonly OperationOptions options;
        private readonly ILogger<OperationProvider> logger;
        private readonly AbpDistributedCacheOptions distributedCacheOptions;
        private readonly IDistributedCache<OperationInfo, string> distributedCache;
        private readonly IGuidGenerator guidGenerator;
        private readonly AsyncLocal<Guid?> OperationId;
        private readonly IDatabase db;

        public OperationProvider(
            ILogger<OperationProvider> logger,
            IOptions<OperationOptions> options,
            IOptions<AbpDistributedCacheOptions> distributedCacheOptions,
            IDistributedCache<OperationInfo, string> distributedCache,
            RedisConnectorHelper redisConnector,
            IGuidGenerator guidGenerator)
        {
            var redis = redisConnector.GetConntction();
            this.db = redis.GetDatabase();
            this.redLockFactory = RedLockFactory.Create(new List<RedLockMultiplexer> { redis });
            this.options = options.Value;
            this.logger = logger;
            this.distributedCacheOptions = distributedCacheOptions.Value;
            this.distributedCache = distributedCache;
            this.guidGenerator = guidGenerator;
            OperationId = new AsyncLocal<Guid?>();
        }

        public virtual void Initialize(Guid? id = null)
        {
            OperationId.Value = id ?? guidGenerator.Create();
        }

        public virtual async Task UpdateOperationAsync(Guid id, Action<OperationInfo> action, TimeSpan? maxSurvivalTime = null)
        {
            var expiryTime = options.DefaultExpiry;
            var waitTime = options.DefaultWait;
            var retryTime = options.DefaultRetry;


            using (var redLock = await redLockFactory.CreateLockAsync(id.ToString(), expiryTime, waitTime, retryTime))
            {
                if (redLock.IsAcquired)
                {
                    var operationInfo = await Get(id);

                    action(operationInfo);

                    await SetAsync(operationInfo, maxSurvivalTime);
                }
                else
                {
                    logger.LogWarning($"Operation {id} is locked.");
                }
            }
        }

        public virtual async Task CreateOperationAsync(Guid id, Action<OperationInfo> action, TimeSpan? maxSurvivalTime = null)
        {
            var expiryTime = options.DefaultExpiry;
            var waitTime = options.DefaultWait;
            var retryTime = options.DefaultRetry;

            this.Initialize(id);

            using (var redLock = await redLockFactory.CreateLockAsync(id.ToString(), expiryTime, waitTime, retryTime))
            {
                if (redLock.IsAcquired)
                {
                    var operationInfo = new OperationInfo(id);

                    action(operationInfo);

                    await SetAsync(operationInfo, maxSurvivalTime ?? options.MaxSurvivalTime);
                }
                else
                {
                    logger.LogWarning($"Operation {id} is locked.");
                }
            }
        }

        public virtual Task<List<Guid>> GetListOperationIdAsync()
        {
            string pattern = $"*k:{distributedCacheOptions.KeyPrefix}*"; // 定義符合特定規則的模式
            var keys = db.Execute("KEYS", pattern); // 使用 KEYS 指令取得符合模式的 key
            var result = new List<Guid>();

            // 返回符合規則的 key
            foreach (string key in (string[])keys)
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
            await distributedCache.RemoveAsync(id.GetCacheKey());
        }

        public virtual Task<OperationInfo?> GetAsync(Guid id)
        {
            return Task.FromResult(distributedCache.Get(id.GetCacheKey()));
        }

        protected virtual async Task<OperationInfo> Get(Guid id)
        {
            var operationInfo = await distributedCache.GetAsync(id.GetCacheKey());

            if (operationInfo == null)
            {
                throw new UserFriendlyException($"Operation {id} not found.");
            }

            var backupKey = id.GetCacheKey().GetOperationBackUpKey();

            await distributedCache.GetAsync(backupKey);

            return operationInfo;
        }

        protected virtual async Task SetAsync(OperationInfo operationInfo, TimeSpan? maxSurvivalTime = null)
        {
            if (maxSurvivalTime != null)
            {
                await distributedCache.SetAsync(operationInfo.GetCacheKey(), operationInfo, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = maxSurvivalTime
                });

                var backupKey = operationInfo.GetCacheKey().GetOperationBackUpKey();

                await distributedCache.SetAsync(backupKey, operationInfo, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = maxSurvivalTime + TimeSpan.FromSeconds(10)
                });
            }

            if (maxSurvivalTime == null)
            {
                await distributedCache.SetAsync(operationInfo.GetCacheKey(), operationInfo);
                await distributedCache.SetAsync(operationInfo.GetCacheKey().GetOperationBackUpKey(), operationInfo);
            }
        }

        protected virtual Guid? RedisKeyParseOperationId(string key)
        {
            if (key.StartsWith("t:"))
            {
                int commaIndex = key.IndexOf(',');
                key = key.Substring(commaIndex + 1);
            }

            key = key.Replace($"c{typeof(OperationInfo)}:,k:{distributedCacheOptions.KeyPrefix}", "");

            string prefix = "Operation:";
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
                else
                {
                    // 如果找到了冒號，表示 id 部分結束於冒號之前
                    return Guid.Parse(key.Substring(idStartIndex, idEndIndex - idStartIndex));
                }
            }

            return null;
        }
    }
}
