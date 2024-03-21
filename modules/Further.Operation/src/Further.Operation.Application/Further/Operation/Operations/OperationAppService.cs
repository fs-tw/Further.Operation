using Further.Operation.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Further.Operation.Operations
{
    public class OperationAppService : ApplicationService, IOperationAppService
    {
        private readonly FurtherOperationOptions options;
        private readonly OperationStore operationStore;
        private readonly IOperationRepository operationRepository;

        public OperationAppService(
            IOptions<FurtherOperationOptions> options,
            OperationStore operationStore,
            IOperationRepository operationRepository)
        {
            this.options = options.Value;
            this.operationStore = operationStore;
            this.operationRepository = operationRepository;
        }

        public Task<List<string>> GetListOwnerTypeAsync()
        {
            return Task.FromResult(options.EntityTypes.Select(x => x.EntityType).ToList());
        }

        public async Task<OperationDto> GetAsync(Guid id)
        {
            var operation = await operationStore.GetAsync(id);

            return operation.ToDto(ObjectMapper);
        }

        public async Task<PagedResultDto<OperationDto>> GetListAsync(OperationDto.GetListInput input)
        {
            var filter = new OperationFilter
            {
                Filter = input.Filter,
                OperationName = input.OperationName,
                IsSuccess = input.IsSuccess,
                CreationTime = new AutoFilterer.Types.Range<DateTime>
                {
                    Min = input.CreationTime?.Min,
                    Max = input.CreationTime?.Max
                },
                OperationOwners = new OperationOwnerFilter
                {
                    EntityType = input.EntityType,
                    EntityId = input.EntityId
                }
            };

            var items = await operationStore.GetListAsync(
                filter: filter,
                maxResultCount: input.MaxResultCount,
                skipCount: input.SkipCount,
                sorting: input.Sorting);

            var count = await operationStore.GetCountAsync(filter: filter);

            return new PagedResultDto<OperationDto>(count, items.ToDtos(ObjectMapper));
        }
    }
}
