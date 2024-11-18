using FluentResults;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Further.Operation
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
        public bool IsSuccess => Result.IsSuccess;

        public List<OperationOwnerInfo> Owners { get; set; } = new();

        public int ExecutionDuration { get; set; } = 0;

        [JsonConstructor]
        public OperationInfo(Guid id)
        {
            Id = id;
        }

        public OperationInfo(Guid id, string operationId, string operationName, IResultBase result, List<OperationOwnerInfo> owners, int executionDuration)
        {
            Id = id;
            OperationId = operationId;
            OperationName = operationName;
            Result = result;
            Owners = owners;
            ExecutionDuration = executionDuration;
        }
    }
}
