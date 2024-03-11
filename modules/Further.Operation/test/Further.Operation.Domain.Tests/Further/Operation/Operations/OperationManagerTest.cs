using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Xunit;

namespace Further.Operation.Operations
{
    public abstract class OperationManagerTest<TStartupModule> : OperationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        public const string TestOperationIType = "TestOperationType";

        protected OperationManager operationManager { get; }

        protected IJsonSerializer jsonSerializer { get; }

        public OperationManagerTest()
        {
            this.operationManager = GetRequiredService<OperationManager>();
            this.jsonSerializer = GetRequiredService<IJsonSerializer>();
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var operationId = "CreateAsync";
            var operationName = "TestOperation";
            var result = Result.Ok();
            var isSuccess = true;
            var executionDuration = 100;

            // Act
            var operation = await operationManager.CreateAsync(
                id: Guid.NewGuid(),
                operationId: operationId,
                operationName: operationName,
                result: result,
                isSuccess: isSuccess,
                executionDuration: executionDuration);

            // Assert
            Assert.NotNull(operation);
            Assert.Equal(operationId, operation.OperationId);
            Assert.Equal(operationName, operation.OperationName);
            Assert.Equal(operation.IsSuccess, isSuccess);
            Assert.Equal(operation.GetResult(jsonSerializer).IsSuccess, result.IsSuccess);
            Assert.Equal(operation.ExecutionDuration, executionDuration);
        }

        [Fact]
        public async Task AddOperationOwnerAsync()
        {
            // Arrange
            var operation = await CreateTestOperation();
            var entityType = TestOperationIType;
            var entityId = Guid.NewGuid();
            var metaData = new Dictionary<string, object> { { "Key", "Value" } };

            // Act
            var updatedOperation = await operationManager.AddOperationOwnerAsync(
                operation: operation,
                entityType: entityType,
                entityId: entityId,
                metaData: metaData);

            // Assert
            Assert.NotNull(updatedOperation);
            Assert.Single(updatedOperation.OperationOwners);
            var operationOwner = updatedOperation.OperationOwners.First();
            Assert.Equal(entityType, operationOwner.EntityType);
            Assert.Equal(entityId, operationOwner.EntityId);
        }

        [Fact]
        public async Task RemoveOperationOwnerAsync()
        {
            // Arrange
            var operation = await CreateTestOperationWithOwner();
            var operationOwner = operation.OperationOwners.First();

            // Act
            var updatedOperation = await operationManager.RemoveOperationOwnerAsync(
                operation: operation,
                operationOwnerId: operationOwner.Id);

            // Assert
            Assert.NotNull(updatedOperation);
            Assert.Empty(updatedOperation.OperationOwners);
        }

        protected async Task<Operation> CreateTestOperation()
        {
            var operationId = "CreateAsync";
            var operationName = "TestOperation";
            var result = Result.Ok();
            var isSuccess = true;
            var executionDuration = 100;

            var operation = await operationManager.CreateAsync(
                id: Guid.NewGuid(),
                operationId: operationId,
                operationName: operationName,
                result: result,
                isSuccess: isSuccess,
                executionDuration: executionDuration);

            return operation;
        }

        protected async Task<Operation> CreateTestOperationWithOwner()
        {
            var operation = await CreateTestOperation();
            var entityType = TestOperationIType;
            var entityId = Guid.NewGuid();
            var metaData = new Dictionary<string, object> { { "Key", "Value" } };

            var updatedOperation = await operationManager.AddOperationOwnerAsync(
                operation: operation,
                entityType: entityType,
                entityId: entityId,
                metaData: metaData);

            return updatedOperation;
        }
    }
}
