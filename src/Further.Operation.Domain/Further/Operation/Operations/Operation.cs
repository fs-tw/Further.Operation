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
using FluentResults;
using Volo.Abp.Json;
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
        public void SetOperationId(string? operationId)
        {
            if (operationId != null)
            {
                Check.Length(operationId, nameof(operationId), OperationConsts.OperationIdMaxLength);
            }

            this.OperationId = operationId;
        }

        public void SetOperationName(string? operationName)
        {
            if (operationName != null)
            {
                Check.Length(operationName, nameof(operationName), OperationConsts.OperationNameMaxLength);
            }

            this.OperationName = operationName;
        }

        public void SetResult(Result result, IJsonSerializer jsonSerializer)
        {
            Check.NotNull(result, nameof(result));

            this.Result = jsonSerializer.Serialize(result);
        }

        public OperationResult GetResult(IJsonSerializer jsonSerializer)
        {
            return jsonSerializer.Deserialize<OperationResult>(this.Result);
        }

        public void SetIsSuccess(bool isSuccess)
        {
            this.IsSuccess = isSuccess;
        }

        public void SetExecutionDuration(int executionDuration)
        {
            this.ExecutionDuration = executionDuration;
        }

        public void AddOperationOwner(OperationOwner operationOwner)
        {
            Check.NotNull(operationOwner, nameof(operationOwner));

            if (this.OperationOwners.Any(o => o.Id == operationOwner.Id))
            {
                throw new UserFriendlyException("OperationOwnerAlreadyExists");
            }

            this.OperationOwners.Add(operationOwner);
        }

        public void RemoveOperationOwner(OperationOwner operationOwner)
        {
            Check.NotNull(operationOwner, nameof(operationOwner));

            RemoveOperationOwner(operationOwner.Id);
        }

        public void RemoveOperationOwner(Guid operationOwnerId)
        {
            var operationOwner = this.OperationOwners.FirstOrDefault(o => o.Id == operationOwnerId);

            if (operationOwner == null)
            {
                throw new UserFriendlyException("OperationOwnerNotExists");
            }

            this.OperationOwners.Remove(operationOwner);
        }

        public void ClearOperationOwners()
        {
            this.OperationOwners.Clear();
        }
        #endregion
    }
}
