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
    public abstract class OperationStoreTest<TStartupModule> : OperationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IOperationRepository operationRepository { get; }
        protected IJsonSerializer jsonSerializer { get; }
        protected TestOperationStoreManager operationManager { get; }

        protected OperationStoreTest()
        {
            operationRepository = GetRequiredService<IOperationRepository>();
            jsonSerializer = GetRequiredService<IJsonSerializer>();
            operationManager = GetRequiredService<TestOperationStoreManager>();
        }

        [Fact]
        public async Task OperationStoreSaveAsync()
        {
            var operationId = await operationManager.SaveAsync();

            var operation = await operationRepository.GetAsync(operationId);

            var result = operation.GetResult(jsonSerializer);

            Assert.NotNull(operation);
            Assert.Equal(
                result.Successes.First().Message,
                "OperationStore保存成功");
        }
    }
}
