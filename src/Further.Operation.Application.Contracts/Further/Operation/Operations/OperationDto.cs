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
        
        #endregion
    }
    public partial class OperationDto
    {
        public partial class GetListInput : PagedAndSortedResultRequestDto
        {
            #region DataTransferObject.EntityDto GetList
             
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
