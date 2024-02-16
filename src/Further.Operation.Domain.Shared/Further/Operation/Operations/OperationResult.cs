using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Further.Operation.Operations
{
    public class OperationResult
    {
        public bool IsFailed { get; set;}

        public bool IsSuccess { get; set; }

        public List<OperationReason> Reasons { get; set; } = new();

        public List<OperationReason> Errors { get; set; } = new();

        public List<OperationReason> Successes { get; set; } = new();
    }

    public class OperationReason
    {
        public string Message { get; set; } = null!;

        public Dictionary<string, object> Metadata { get; set; } = new();
    }
}
