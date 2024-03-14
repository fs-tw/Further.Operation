using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Abp.Operation
{
    public class OperationExpiredEvent
    {
        public OperationInfo OperationInfo { get; set; } = null!;
    }
}
