using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Operation.Operations
{
    public class OperationOwnerFilter : OperationFilterBase<Operation>
    {
        public Guid? EntityId { get; set; }

        [StringFilterOptions(StringFilterOption.Equals)]
        public string? EntityType { get; set; }
    }
}
