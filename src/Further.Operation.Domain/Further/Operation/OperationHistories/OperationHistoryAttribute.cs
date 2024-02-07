using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Operation.OperationHistories
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OperationHistoryAttribute : Attribute
    {
        public string? EntityType { get; set; }
    }
}
