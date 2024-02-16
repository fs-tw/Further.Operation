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
    public partial class Operation : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Operation()
        {
            this.OperationOwners = new Collection<OperationOwner>();
            #region EFCore.Entities Default Constructor
            
            #endregion
        }
        public Operation(Guid id) : this()
        {
            this.Id = id;
        }
        public Operation(Guid id, string? operationId, string? operationName, string result, bool isSuccess, int executionDuration, Guid? tenantId) : this()
        {
            this.Id = id;
            this.OperationId = operationId;
            this.OperationName = operationName;
            this.Result = result;
            this.IsSuccess = isSuccess;
            this.ExecutionDuration = executionDuration;
            this.TenantId = tenantId;
        }

        public string? OperationId { get; set; }

        public string? OperationName { get; set; }

        public string Result { get; set; }

        public bool IsSuccess { get; set; }

        public int ExecutionDuration { get; set; }

        public Guid? TenantId { get; set; }

        public virtual ICollection<OperationOwner> OperationOwners { get; set; }

        #region Extensibility Method Definitions


        #endregion

        #region EFCore.Entities
        
        #endregion
    }
}
