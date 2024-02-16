using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
#region EFCore.Entities Using

#endregion

namespace Further.Operation.Operations
{
    public partial class OperationOwner : FullAuditedEntity<Guid>, IMultiTenant
    {
        public OperationOwner()
        {
            #region EFCore.Entities Default Constructor
            
            #endregion
        }
        public OperationOwner(Guid id) : this()
        {
            this.Id = id;
        }
        public OperationOwner(Guid id, Guid operationId, string entityType, Guid entityId, string metaData, Guid? tenantId) : this()
        {
            this.Id = id;
            this.OperationId = operationId;
            this.EntityType = entityType;
            this.EntityId = entityId;
            this.MetaData = metaData;
            this.TenantId = tenantId;
        }

        public Guid OperationId { get; set; }

        public string EntityType { get; set; }

        public Guid EntityId { get; set; }

        public string MetaData { get; set; }

        public Guid? TenantId { get; set; }

        #region Extensibility Method Definitions


        #endregion

        #region EFCore.Entities
        
        #endregion
    }
}
