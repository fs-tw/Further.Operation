using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Abp.Operation
{
    public abstract class OperationAttributeBase : Attribute
    {
        public Dictionary<string, object> Metadata { get; set; } = new();
        public abstract void UpdateOperationInfo(OperationInfo operationInfo);
    }
}
