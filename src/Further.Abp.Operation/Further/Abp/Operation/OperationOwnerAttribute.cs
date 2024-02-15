using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Abp.Operation
{
    public class OperationOwnerAttribute : OperationAttributeBase
    {
        public string EntityType { get; set; } = null!;

        public override void UpdateOperationInfo(OperationInfo operationInfo, object methodResult)
        {
            
        }
    }
}
