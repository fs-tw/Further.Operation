using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace Further.Operation.Operations
{
    public abstract class OperationAppServiceTest<TStartupModule> : OperationApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IOperationAppService operationAppService;

        private readonly TestOperationStoreManager testOperationStoreManager;

        public OperationAppServiceTest()
        {
            operationAppService = GetRequiredService<IOperationAppService>();
            testOperationStoreManager = GetRequiredService<TestOperationStoreManager>();
        }


        [Fact]
        public async Task GetListAsync()
        {
            var input = new OperationDto.GetListInput
            {
                MaxResultCount = 10,
                SkipCount = 0
            };

            await testOperationStoreManager.SaveAsync();
            await testOperationStoreManager.SaveAsync();

            Task.Delay(6000).Wait();

            var result = await operationAppService.GetListAsync(input);

            Assert.Equal(2, result.TotalCount);
        }

        [Fact]
        public async Task GetAsync()
        {
            var operationId = await testOperationStoreManager.SaveAsync();

            Task.Delay(6000).Wait();

            var result = await operationAppService.GetAsync(operationId);

            Assert.NotNull(result);
            Assert.Equal(operationId, result.Id);
        }

        [Fact]
        public async Task GetOwnerAsync()
        {
            var test = await testOperationStoreManager.SaveOwnerAsync();

            Task.Delay(6000).Wait();

            var result = await operationAppService.GetAsync(test.OpenInfoId);

            Assert.NotNull(result);
            Assert.Equal(test.OpenInfoId, result.Id);
            Assert.Equal(1, result.OperationOwners.Count);
        }
    }
}
