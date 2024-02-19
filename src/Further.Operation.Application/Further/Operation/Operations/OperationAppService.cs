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
        private readonly IOperationRepository operationRepository;

        public OperationAppService(
            IOperationRepository operationRepository)
        {
            this.operationRepository = operationRepository;
        }

        public async Task<OperationDto> GetAsync(Guid id)
        {
            var operation = await operationRepository.GetAsync(id);

            return operation.ToDto(ObjectMapper);
        }

        public async Task<PagedResultDto<OperationDto>> GetListAsync(OperationDto.GetListInput input)
        {
            var filter = new OperationFilter
            {
                Filter = input.Filter,
                IsSuccess = input.IsSuccess,
                ExecutionDuration = new AutoFilterer.Types.Range<int>
                {
                    Min = input.ExecutionDuration?.Min,
                    Max = input.ExecutionDuration?.Max
                },
                OperationOwners = new OperationOwnerFilter
                {
                    EntityType = input.EntityType,
                    EntityId = input.EntityId
                }
            };

            var items = await operationRepository.GetListAsync(
                specification: filter,
                includeDetails: true,
                maxResultCount: input.MaxResultCount,
                skipCount: input.SkipCount,
                sorting: input.Sorting);

            var count = await operationRepository.GetCountAsync(specification: filter);

            return new PagedResultDto<OperationDto>(count, items.ToDtos(ObjectMapper));
        }
    }
}
