using FluentResults;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Further.Abp.Operation
{

    [Serializable]
    public class OperationInfo
    {
        public Guid Id { get; private set; }
        public string? OperationId { get; set; }

        public string? OperationName { get; set; }

        public IResultBase Result { get; set; } = FluentResults.Result.Ok();

        public Result GetResult()
        {
            return (Result)Result;
        }
        public bool IsSuccess => this.Result.IsSuccess;

        public List<OperationOwnerInfo> Owners { get; set; } = new();

        public int ExecutionDuration { get; set; } = 0;

        [JsonConstructor]
        public OperationInfo(Guid id)
        {
            this.Id = id;
        }

        public OperationInfo(Guid id, string? operationId, string? operationName, Result result, List<OperationOwnerInfo> owners, int executionDuration)
        {
            this.Id = id;
            this.OperationId = operationId;
            this.OperationName = operationName;
            this.Result = result;
            this.Owners = owners;
            this.ExecutionDuration = executionDuration;
        }
    }
}
