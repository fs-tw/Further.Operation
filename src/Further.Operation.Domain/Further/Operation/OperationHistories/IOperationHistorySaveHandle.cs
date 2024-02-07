using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Further.Operation.OperationHistories
{
    public interface IOperationHistorySaveHandle : IDisposable
    {
        Task SaveAsync();
    }
}
