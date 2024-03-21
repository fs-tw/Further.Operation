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
            nameof(Operation.OperationId),
            nameof(Operation.OperationName)
            )]
        [StringFilterOptions(StringFilterOption.Contains)]
        public string? Filter { get; set; }

        [StringFilterOptions(StringFilterOption.Equals)]
        public string? OperationId { get; set; }

        [StringFilterOptions(StringFilterOption.Equals)]
        public string? OperationName { get; set; }

        public bool? IsSuccess { get; set; }

        public OperationOwnerFilter? OperationOwners { get; set; }

        public Range<int>? ExecutionDuration { get; set; }

        [CompareTo(nameof(Operation.CreationTime))]
        public Range<DateTime>? CreationTime { get; set; }
    }
}
