using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Operation
{
    public class OperationCorelationInfo
    {
        public string CorelationType { get; set; } = null!;

        public Guid CorelationId { get; set; }

        public Dictionary<string, object> MetaData { get; set; } = new Dictionary<string, object>();
    }
}
