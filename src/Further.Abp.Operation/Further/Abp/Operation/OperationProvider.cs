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
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Further.Abp.Operation
{
    public class OperationProvider : IOperationProvider, ITransientDependency
    {
        private readonly RedLockFactory redLockFactory;
        private readonly OperationOptions options;
        private readonly ILogger<OperationProvider> logger;
        private readonly IDistributedCache<CacheOperationInfo, string> distributedCache;

        /// <summary>
        /// 待後續調整jsonSerialization使用，進一步移除CacheOperationInfo這轉換物件
        /// </summary>
        public class CacheOperationInfo : OperationInfo
        {
            public new OperationResult Result { get; set; }

            public new List<OperationOwnerInfo> Owners { get; set; } = new();

            [JsonConstructor]
            public CacheOperationInfo(Guid id) : base(id)
            {
            }

            public CacheOperationInfo(OperationInfo operationInfo) : base(operationInfo.Id)
            {
                this.OperationId = operationInfo.OperationId;
                this.OperationName = operationInfo.OperationName;
                this.ExecutionDuration = operationInfo.ExecutionDuration;
                this.Result = new OperationResult(operationInfo.Result);
                this.Owners.AddRange(operationInfo.Owners);
            }

            public OperationInfo GetOperationInfo()
            {
                var operation = new OperationInfo(
                    id: this.Id,
                    operationId: this.OperationId,
                    operationName: this.OperationName,
                    result: this.Result.CopyToResult(),
                    owners: this.Owners,
                    executionDuration: this.ExecutionDuration);

                return operation;
            }
        }

        public OperationProvider(
            ILogger<OperationProvider> logger,
            IOptions<OperationOptions> options,
            IDistributedCache<CacheOperationInfo, string> distributedCache,
            RedisConnectorHelper redisConnector)
        {
            var redis = redisConnector.GetConntction();
            this.redLockFactory = RedLockFactory.Create(new List<RedLockMultiplexer> { redis });
            this.options = options.Value;
            this.logger = logger;
            this.distributedCache = distributedCache;
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

        public virtual Task<OperationInfo?> GetAsync(Guid id)
        {
            return Task.FromResult(distributedCache.Get(id.GetCacheKey())?.GetOperationInfo());
        }

        protected virtual async Task<OperationInfo> GetOrCreate(Guid id)
        {
            var operationCacheInfo = await distributedCache.GetAsync(id.GetCacheKey());
            OperationInfo? operationInfo = operationCacheInfo?.GetOperationInfo();

            if (operationInfo == null)
            {
                operationInfo = new OperationInfo(id);
            }

            return operationInfo;
        }

        protected virtual async Task SetAsync(OperationInfo operationInfo)
        {
            await distributedCache.SetAsync(operationInfo.GetCacheKey(), new CacheOperationInfo(operationInfo), new DistributedCacheEntryOptions
            {
                SlidingExpiration = options.MaxSurvivalTime
            });
        }
    }
}
