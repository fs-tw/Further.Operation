using FluentResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Operation
{
    public class BasicOperationInfo
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public IResultBase Result { get; set; } = FluentResults.Result.Ok();
        public List<OperationCorrelationInfo> Correlations { get; set; } = new List<OperationCorrelationInfo>();

        public BasicOperationInfo()
            : this(Guid.NewGuid())
        {
        }
        public BasicOperationInfo(Guid id)
        {
            Id = id;
        }
        public BasicOperationInfo(BasicOperationInfo operationInfo)
        {
            Id = operationInfo.Id;
            Name = operationInfo.Name;
            Result = operationInfo.Result;
            Correlations = operationInfo.Correlations;
        }
    }

    public class OperationCorrelationInfo
    {
        public string Type { get; set; } = null!;

        public Guid Id { get; set; }

        public Dictionary<string, object> MetaData { get; set; } = new Dictionary<string, object>();
    }
}
