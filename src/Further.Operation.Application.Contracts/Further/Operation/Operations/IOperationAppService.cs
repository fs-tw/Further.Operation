using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Further.Operation.Operations
{
    public interface IOperationAppService
    {
        Task<OperationDto> GetAsync(Guid id);

        Task<PagedResultDto<OperationDto>> GetListAsync(OperationDto.GetListInput input);
    }
}
