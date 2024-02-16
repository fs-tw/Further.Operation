using FluentResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Abp.Operation
{
    public class OperationInfo
    {
        public Guid Id { get; private set; }
        public string? OperationId { get; set; }

        public string? OperationName { get; set; }

        public Result Result { get; } = Result.Ok();

        public bool IsSuccess => this.Result.IsSuccess;

        public List<OperationOwnerInfo> Owners { get; } = new();

        public int ExecutionDuration { get; set; }

        public OperationInfo(Guid id)
        {
            this.Id = id;
        }
    }
}
