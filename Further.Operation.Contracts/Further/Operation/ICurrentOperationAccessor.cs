using FluentResults;
using System;
using System.Collections.Generic;

namespace Further.Operation
{
    public interface ICurrentOperationAccessor
    {
        BasicOperationInfo? Current { get; set; }

        void Initial(Guid id);
    }

}
