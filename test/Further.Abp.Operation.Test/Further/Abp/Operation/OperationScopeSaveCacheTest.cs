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
        private readonly IDistributedCache<OperationInfo, Guid> distributedCache;

        public OperationScopeSaveCacheTest()
        {
            this.distributedCache = this.GetRequiredService<IDistributedCache<OperationInfo, Guid>>();
        }

        [Fact]
        public async Task OperationScopeSaveCache()
        {
            var options = new OperationScopeOptions
            {
                SaveToCache = true
            };

            var value = new OperationInfoInitializeValue
            {
                OperationName = "OperationScopeSaveCache"
            };

            try
            {
                using (var operationScope = operationScopeProvider.Begin(options, value))
                {
                    await operationScope.CompleteAsync();
                }
            }
            catch (OperationScopeCompleteException ex)
            {
                var operationInfo = await distributedCache.GetAsync(ex.OperationInfo.Id);

                Assert.NotNull(operationInfo);
            }
        }
    }
}
