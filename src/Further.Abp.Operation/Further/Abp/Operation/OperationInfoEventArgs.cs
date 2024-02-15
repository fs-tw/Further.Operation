using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;

namespace Further.Abp.Operation
{
    public class OperationInfoEventArgs : EventArgs
    {
        public IOperationScope OperationScope { get; }

        public OperationInfoEventArgs([NotNull] IOperationScope operationScope)
        {
            Check.NotNull(operationScope, nameof(operationScope));

            OperationScope = operationScope;
        }
    }
}
