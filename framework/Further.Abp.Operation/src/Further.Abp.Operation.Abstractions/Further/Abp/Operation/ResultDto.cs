using System.Collections.Generic;

namespace Further.Abp.Operation
{
    internal class ResultDto
    {
        internal class Success
        {

            public string? Message { get; set; }

            public Dictionary<string, object>? Metadata { get; set; }
        }
        internal class Error
        {

            public string? Message { get; set; }

            public Dictionary<string, object>? Metadata { get; set; }
            public List<Error>? Reasons { get; set; }
        }
        public bool IsFailed { get; set; }
        public bool IsSuccess { get; set; }
        public List<Error>? Errors { get; set; }
        public List<Success>? Successes { get; set; }
    }
}
