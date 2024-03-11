using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Further.Operation.Operations
{
    public class OperationController : AbpControllerBase, IRemoteService
    {
        private readonly IOperationAppService operationAppService;

        public OperationController(
            IOperationAppService operationAppService)
        {
            this.operationAppService = operationAppService;
        }

        public Task<OperationDto> GetAsync(Guid id)
        {
            return operationAppService.GetAsync(id);
        }

        public Task<PagedResultDto<OperationDto>> GetListAsync(OperationDto.GetListInput input)
        {
            return operationAppService.GetListAsync(input);
        }
    }
}
