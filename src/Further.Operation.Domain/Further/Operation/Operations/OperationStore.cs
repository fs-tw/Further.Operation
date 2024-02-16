using Further.Abp.Operation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Further.Operation.Operations
{
    [Dependency(ReplaceServices = true)]
    public class OperationStore : IOperationStore, ITransientDependency
    {
        private readonly IOperationRepository operationRepository;
        private readonly OperationManager operationManager;
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly ICurrentTenant currentTenant;

        public OperationStore(
            IOperationRepository operationRepository,
            OperationManager operationManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICurrentTenant currentTenant)
        {
            this.operationRepository = operationRepository;
            this.operationManager = operationManager;
            this.unitOfWorkManager = unitOfWorkManager;
            this.currentTenant = currentTenant;
        }

        [UnitOfWork]
        public async Task SaveAsync(OperationInfo? operationInfo, CancellationToken cancellationToken = default)
        {
            if (operationInfo == null) return;

            var operation = await operationManager.CreateAsync(
                id: operationInfo.Id,
                operationId: operationInfo?.OperationId,
                operationName: operationInfo?.OperationName,
                result: operationInfo?.Result,
                isSuccess: (bool)operationInfo?.IsSuccess,
                executionDuration: (int)operationInfo?.ExecutionDuration,
                tenantId: currentTenant.Id);

            foreach (var owner in operationInfo?.Owners)
            {
                operation = await operationManager.AddOperationOwnerAsync(
                    operation: operation,
                    entityType: owner.EntityType,
                    entityId: owner.EntityId,
                    metaData: owner.MetaData);
            }

            await operationRepository.InsertAsync(operation, cancellationToken: cancellationToken);
        }
    }
}
