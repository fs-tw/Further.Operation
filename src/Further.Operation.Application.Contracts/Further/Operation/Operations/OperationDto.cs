using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Further.Operation.Operations
{

    public partial class OperationDto : FullAuditedEntityDto<Guid>
    {

        public string OperationId { get; set; }

        public string OperationName { get; set; }

        public string Result { get; set; }

        public bool IsSuccess { get; set; }

        public int ExecutionDuration { get; set; }

        #region DataTransferObject.EntityDto.Property
        public OperationResultDto OperationResult { get; set; }

        public List<OperationOwnerDto> OperationOwners { get; set; }
        #endregion
    }
    public partial class OperationDto
    {
        public partial class GetListInput : PagedAndSortedResultRequestDto
        {
            #region DataTransferObject.EntityDto GetList
            public string? Filter { get; set; }

            public bool? IsSuccess { get; set; }

            public FurtherOperationRange<int>? ExecutionDuration { get; set; }

            public string? EntityType { get; set; }

            public Guid? EntityId { get; set; }
            #endregion
        }
    }

    public partial class OperationDto
    {
        public partial class Create
        {
            #region DataTransferObject.EntityDto Create
            public string OperationId { get; set; }

            public string OperationName { get; set; }

            public string Result { get; set; }

            public bool IsSuccess { get; set; }

            public int ExecutionDuration { get; set; }
            #endregion
        }
    }
    public partial class OperationDto
    {
        public partial class Update
        {
            #region DataTransferObject.EntityDto Update
            public string OperationId { get; set; }

            public string OperationName { get; set; }

            public string Result { get; set; }

            public bool IsSuccess { get; set; }

            public int ExecutionDuration { get; set; }
            #endregion
        }
    }
    #region DataTransferObject.EntityDto

    #endregion
}
