using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Further.Operation
{
    public interface IOperationExpiredProvider
    {
        Task ExecuteAsync(OperationInfo operationInfo);
    }
}
