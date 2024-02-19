using Further.Abp.Operation;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Further.Operation.Operations
{
    [Dependency(ReplaceServices = true)]
    public class OperationStore : CacheOperationStore, ITransientDependency
    {
        private readonly IDistributedCache<CacheOperationInfo, string> distributedCache;
        private readonly IOperationRepository operationRepository;
        private readonly OperationManager operationManager;
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly ICurrentTenant currentTenant;

        public OperationStore(
            IDistributedCache<CacheOperationInfo, string> distributedCache,
            IOperationRepository operationRepository,
            OperationManager operationManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICurrentTenant currentTenant) :
            base(distributedCache)
        {
            this.distributedCache = distributedCache;
            this.operationRepository = operationRepository;
            this.operationManager = operationManager;
            this.unitOfWorkManager = unitOfWorkManager;
            this.currentTenant = currentTenant;
        }

        public override OperationInfo? Get(Guid id)
        {
            var backupKey = OperationConsts.GetOperationBackUpKey(id.GetCacheKey());

            distributedCache.Get(backupKey);

            return base.Get(id);
        }

        public override async Task SaveAsync(OperationInfo? operationInfo, OperationScopeOptions options, CancellationToken cancellationToken = default)
        {
            if (operationInfo == null) return;

            var backupKey = OperationConsts.GetOperationBackUpKey(operationInfo.GetCacheKey());

            await distributedCache.SetAsync(backupKey, new CacheOperationInfo(operationInfo), new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(options.MaxSurvivalTime + 10)
            });

            await base.SaveAsync(operationInfo, options, cancellationToken);
        }
    }
}
