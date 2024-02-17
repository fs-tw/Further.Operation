using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Further.Abp.Operation
{
    public class CacheOperationStore : IOperationStore, ITransientDependency
    {
        private readonly IDistributedCache<OperationInfo, Guid> distributedCache;

        public CacheOperationStore(
            IDistributedCache<OperationInfo, Guid> distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public OperationInfo? Get(Guid id)
        {
            return distributedCache.Get(id);
        }

        public async Task SaveAsync(OperationInfo? operationInfo, OperationScopeOptions options, CancellationToken cancellationToken = default)
        {
            if (operationInfo == null) return;

            await distributedCache.SetAsync(operationInfo.Id, operationInfo, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(options.MaxSurvivalTime)
            });
        }
    }
}
