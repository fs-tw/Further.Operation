using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Xunit;

namespace Further.Abp.Operation
{
    public class OperationScopeSaveCacheTest : OperationTestBase
    {
        private readonly IOperationStore operationStore;

        public OperationScopeSaveCacheTest()
        {
            this.operationStore = GetRequiredService<IOperationStore>();
        }

        [Fact]
        public async Task OperationScopeSaveCache()
        {
            var value = new OperationInfoInitializeValue
            {
                OperationId = "OperationScopeSaveCache",
                OperationName = "OperationScopeSaveCache"
            };

            try
            {
                using (var operationScope = operationScopeProvider.Begin(value: value))
                {
                    await operationScope.CompleteAsync();
                }
            }
            catch (OperationScopeCompleteException ex)
            {
                var operationInfo = operationStore.Get(ex.OperationInfo.Id);

                Assert.NotNull(operationInfo);
            }
        }
    }
}
