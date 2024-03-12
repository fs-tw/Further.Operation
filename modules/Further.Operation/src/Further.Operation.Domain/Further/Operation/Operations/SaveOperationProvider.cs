using Further.Abp.Operation;
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
    public class SaveOperationProvider : ITransientDependency
    {
        private readonly IDistributedCache<OperationInfo, string> distributedCache;
        private readonly IOperationRepository operationRepository;
        private readonly OperationManager operationManager;
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly ICurrentTenant currentTenant;

        public SaveOperationProvider(
            IDistributedCache<OperationInfo, string> distributedCache,
            IOperationRepository operationRepository,
            OperationManager operationManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICurrentTenant currentTenant)
        {
            this.distributedCache = distributedCache;
            this.operationRepository = operationRepository;
            this.operationManager = operationManager;
            this.unitOfWorkManager = unitOfWorkManager;
            this.currentTenant = currentTenant;
        }

        public async Task SaveExpiredCacheOperation(string key)
        {
            var backKey = key.GetOperationBackUpKey();

            var operationInfo = await distributedCache.GetAsync(backKey);

            if (operationInfo == null) return;

            await distributedCache.RemoveAsync(backKey);

            var operation = await operationManager.CreateAsync(
                id: operationInfo.Id,
                operationId: operationInfo!.OperationId!,
                operationName: operationInfo!.OperationName!,
                result: operationInfo!.Result!,
                isSuccess: operationInfo!.IsSuccess!,
                executionDuration: operationInfo!.ExecutionDuration!,
                tenantId: currentTenant.Id);

            foreach (var owner in operationInfo!.Owners!)
            {
                operation = await operationManager.AddOperationOwnerAsync(
                    operation: operation,
                    entityType: owner.EntityType,
                    entityId: owner.EntityId,
                    metaData: owner.MetaData);
            }

            using (var uow = unitOfWorkManager.Begin(requiresNew: true))
            {
                await operationRepository.InsertAsync(operation);
                await uow.CompleteAsync();
            }
        }
    }
}
