using FluentResults;
using Further.Abp.Operation;
using Further.Operation.Options.TypeDefinitions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;

namespace Further.Operation.Operations
{
    public class OperationManager : DomainService
    {
        private readonly IJsonSerializer jsonSerializer;
        private readonly IOperationOwnerTypeDefinitionStore operationOwnerTypeDefinitionStore;

        public OperationManager(
            IJsonSerializer jsonSerializer,
            IOperationOwnerTypeDefinitionStore operationOwnerTypeDefinitionStore)
        {
            this.jsonSerializer = jsonSerializer;
            this.operationOwnerTypeDefinitionStore = operationOwnerTypeDefinitionStore;
        }

        public virtual Task<Operation> CreateAsync(Guid id, string operationId, string operationName, Result result, bool isSuccess, int executionDuration = 0, Guid? tenantId = null)
        {
            var operation = new Operation(id);

            operation.SetOperationId(operationId);
            operation.SetOperationName(operationName);
            operation.SetResult(result, jsonSerializer);
            operation.SetIsSuccess(isSuccess);
            operation.SetExecutionDuration(executionDuration);

            operation.TenantId = tenantId;

            return Task.FromResult(operation);
        }

        public virtual async Task<Operation> AddOperationOwnerAsync(Operation operation, string entityType, Guid entityId, Dictionary<string, object>? metaData = null)
        {
            await operationOwnerTypeDefinitionStore.VaildEntityTypeAsync(entityType);

            var operationOwner = new OperationOwner(GuidGenerator.Create());

            operationOwner.SetEntityType(entityType);
            operationOwner.SetEntityId(entityId);
            operationOwner.SetMetaData(metaData, jsonSerializer);

            operation.AddOperationOwner(operationOwner);

            return operation;
        }

        public virtual Task<Operation> RemoveOperationOwnerAsync(Operation operation, Guid operationOwnerId)
        {
            operation.RemoveOperationOwner(operationOwnerId);

            return Task.FromResult(operation);
        }

        public async virtual Task<Operation> CreateAsync(OperationInfo operationInfo)
        {
            var operation = await CreateAsync(
                id: operationInfo.Id,
                operationId: operationInfo!.OperationId!,
                operationName: operationInfo!.OperationName!,
                result: operationInfo!.GetResult()!,
                isSuccess: operationInfo!.IsSuccess!,
                executionDuration: operationInfo!.ExecutionDuration!,
                tenantId: CurrentTenant.Id);

            foreach (var owner in operationInfo!.Owners!)
            {
                operation = await AddOperationOwnerAsync(
                    operation: operation,
                    entityType: owner.EntityType,
                    entityId: owner.EntityId,
                    metaData: owner.MetaData);
            }

            return operation;
        }
    }
}
