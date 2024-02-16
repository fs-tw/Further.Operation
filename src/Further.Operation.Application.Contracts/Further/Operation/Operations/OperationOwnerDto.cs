using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Further.Operation.Operations
{

    public partial class OperationOwnerDto : FullAuditedEntityDto<Guid>
    {

        public System.Guid OperationId { get; set; }

        public string EntityType { get; set; }

        public System.Guid EntityId { get; set; }

        public string MetaData { get; set; }

        #region DataTransferObject.EntityDto.Property
        
        #endregion
    }
    #region DataTransferObject.EntityDto
    
    #endregion
}
