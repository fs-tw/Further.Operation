using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Operation.Operations
{
    public class OperationResultDto
    {
        public bool IsFailed { get; set; }

        public bool IsSuccess { get; set; }

        public List<OperationReasonDto> Reasons { get; set; } = new();

        public List<OperationReasonDto> Errors { get; set; } = new();

        public List<OperationReasonDto> Successes { get; set; } = new();
    }

    public class OperationReasonDto
    {
        public string Message { get; set; } = null!;

        public Dictionary<string, object> Metadata { get; set; } = new();
    }
}
