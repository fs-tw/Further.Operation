using FluentResults;
using Further.Operation.Operations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
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
        private readonly IDistributedCache<OperationInfo, string> distributedCache;
        private readonly IGuidGenerator guidGenerator;
        private readonly AsyncLocal<Guid?> OperationId;

        public OperationProvider(
            ILogger<OperationProvider> logger,
            IOptions<OperationOptions> options,
            IDistributedCache<OperationInfo, string> distributedCache,
            RedisConnectorHelper redisConnector,
            IGuidGenerator guidGenerator)
        {
            var redis = redisConnector.GetConntction();
            this.redLockFactory = RedLockFactory.Create(new List<RedLockMultiplexer> { redis });
            this.options = options.Value;
            this.logger = logger;
            this.distributedCache = distributedCache;
            this.guidGenerator = guidGenerator;
            OperationId = new AsyncLocal<Guid?>();
        }

        public virtual void Initialize(Guid? id = null)
        {
            OperationId.Value = id ?? guidGenerator.Create();
        }

        public virtual async Task ModifyOperationAsync(Action<OperationInfo> action, TimeSpan? expiry = null, TimeSpan? wait = null, TimeSpan? retry = null)
        {
            await ModifyOperationAsync(CurrentId ?? guidGenerator.Create(), action, expiry, wait, retry);
        }

        public virtual async Task ModifyOperationAsync(Guid id, Action<OperationInfo> action, TimeSpan? expiry = null, TimeSpan? wait = null, TimeSpan? retry = null)
        {
            var expiryTime = expiry ?? options.DefaultExpiry;
            var waitTime = wait ?? options.DefaultWait;
            var retryTime = retry ?? options.DefaultRetry;


            using (var redLock = await redLockFactory.CreateLockAsync(id.ToString(), expiryTime, waitTime, retryTime))
            {
                if (redLock.IsAcquired)
                {
                    var operationInfo = await GetOrCreate(id);

                    action(operationInfo);

                    await SetAsync(operationInfo);
                }
                else
                {
                    logger.LogWarning($"Operation {id} is locked.");
                }
            }
        }

        public virtual async Task RemoveAsync(Guid id)
        {
            await distributedCache.RemoveAsync(id.GetCacheKey());
        }

        public virtual async Task RemoveAsync(string id)
        {
            await distributedCache.RemoveAsync(id);
        }

        public virtual Task<OperationInfo?> GetAsync(Guid id)
        {
            return Task.FromResult(distributedCache.Get(id.GetCacheKey()));
        }

        public async Task<OperationInfo?> GetAsync(string id)
        {
            var operationInfo = await distributedCache.GetAsync(id);

            return operationInfo;
        }

        protected virtual async Task<OperationInfo> GetOrCreate(Guid id)
        {
            var operationCacheInfo = await distributedCache.GetAsync(id.GetCacheKey());
            OperationInfo? operationInfo = operationCacheInfo;

            if (operationInfo == null)
            {
                operationInfo = new OperationInfo(id);
            }

            var backupKey = id.GetCacheKey().GetOperationBackUpKey();

            await distributedCache.GetAsync(backupKey);

            return operationInfo;
        }

        protected virtual async Task SetAsync(OperationInfo operationInfo)
        {
            await distributedCache.SetAsync(operationInfo.GetCacheKey(), operationInfo, new DistributedCacheEntryOptions
            {
                SlidingExpiration = options.MaxSurvivalTime
            });

            var backupKey = operationInfo.GetCacheKey().GetOperationBackUpKey();

            await distributedCache.SetAsync(backupKey, operationInfo, new DistributedCacheEntryOptions
            {
                SlidingExpiration = options.MaxSurvivalTime + TimeSpan.FromSeconds(10)
            });
        }
    }
}
