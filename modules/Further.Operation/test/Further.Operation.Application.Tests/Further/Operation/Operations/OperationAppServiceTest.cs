using FluentResults;
using Further.Abp.Operation;
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
        private readonly IOperationProvider operationProvider;

        public OperationAppServiceTest()
        {
            operationAppService = GetRequiredService<IOperationAppService>();
            operationProvider = GetRequiredService<IOperationProvider>();
        }


        [Fact]
        public async Task GetListAsync()
        {
            var input = new OperationDto.GetListInput
            {
                MaxResultCount = 10,
                SkipCount = 0
            };

            await CreateTestOperation();

            Task.Delay(8000).Wait();

            var result = await operationAppService.GetListAsync(input);

            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task GetAsync()
        {
            var operationId = await CreateTestOperation();

            Task.Delay(8000).Wait();

            var result = await operationAppService.GetAsync(operationId);

            Assert.NotNull(result);
            Assert.Equal(operationId, result.Id);
        }

        [Fact]
        public async Task GetOwnerAsync()
        {
            var test = await CreateTestOperation();

            Task.Delay(8000).Wait();

            var result = await operationAppService.GetAsync(test);

            Assert.NotNull(result);
            Assert.Equal(test, result.Id);
            Assert.Equal(1, result.OperationOwners.Count);
        }

        protected async Task<Guid> CreateTestOperation()
        {
            var operationId = Guid.NewGuid();

            await operationProvider.ModifyOperationAsync(operationId, operationInfo =>
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

            return operationId;
        }
    }
}
