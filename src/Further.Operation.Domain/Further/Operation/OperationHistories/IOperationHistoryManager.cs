using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Operation.OperationHistories
{
    public interface IOperationHistoryManager
    {
        OperationHistory? Current { get; }

        IOperationHistorySaveHandle BeginScope();
    }
}
