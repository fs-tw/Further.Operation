using FluentResults;
using Further.Abp.Operation;
using Microsoft.Extensions.Caching.Distributed;
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
    public abstract class OperationSaveTest<TStartupModule> : OperationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IOperationRepository operationRepository { get; }
        protected IJsonSerializer jsonSerializer { get; }
        protected IOperationProvider operationProvider { get; }

        protected IDistributedCache distributedCache { get; }

        protected OperationSaveTest()
        {
            operationRepository = GetRequiredService<IOperationRepository>();
            jsonSerializer = GetRequiredService<IJsonSerializer>();
            operationProvider = GetRequiredService<IOperationProvider>();
            distributedCache = GetRequiredService<IDistributedCache>();
        }

        [Fact]
        public async Task OperationSaveAsync()
        {
            var operationId = Guid.NewGuid();

            await operationProvider.CreateOperationAsync(operationId, operationInfo =>
            {
                operationInfo.OperationId = operationId.ToString();
                operationInfo.OperationName = "TestOperation";
                operationInfo.Result.WithSuccess(new Success("OperationStore保存成功"));
                operationInfo.Owners.Add(new OperationOwnerInfo
                {
                    EntityType = "TestOperationType",
                    EntityId = Guid.NewGuid()
                });
            });

            Task.Delay(6000).Wait();

            var operation = await operationRepository.GetAsync(operationId);

            var result = operation.GetResult(jsonSerializer);

            Assert.NotNull(operation);
            Assert.Equal(
                result.Successes.First().Message,
                "OperationStore保存成功");
        }

        [Fact]
        public async Task OperationGetListAsync()
        {
            var operationId = Guid.NewGuid();

            await operationProvider.CreateOperationAsync(operationId, operationInfo =>
            {
                operationInfo.OperationId = operationId.ToString();
                operationInfo.OperationName = "TestOperation";
                operationInfo.Result.WithSuccess(new Success("OperationStore保存成功"));
                operationInfo.Owners.Add(new OperationOwnerInfo
                {
                    EntityType = "TestOperationType",
                    EntityId = Guid.NewGuid()
                });
            });

            var ids = await operationProvider.ListIdsAsync();

            Assert.NotNull(ids);
            Assert.Contains(operationId, ids);
        }

        [Fact]
        public async Task TestCacheAsync()
        {
            await distributedCache.SetStringAsync("test", "test");
        }

    }
}
