using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Operation.Operations
{
    public class OperationFilter : OperationFilterBase<Operation>
    {
        [CompareTo(
            nameof(Operation.Id),
            nameof(Operation.OperationName)
            )]
        [StringFilterOptions(StringFilterOption.Contains)]
        public string? Filter { get; set; }

        public bool? IsSuccess { get; set; }

        public OperationOwnerFilter? OperationOwners { get; set; }

        public Range<int>? ExecutionDuration { get; set; }
    }
}
