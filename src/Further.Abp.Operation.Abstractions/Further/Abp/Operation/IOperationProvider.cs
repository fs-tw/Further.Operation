using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Further.Abp.Operation
{
    public interface IOperationProvider
    {
        Task ModifyOperationAsync(Guid id, Action<OperationInfo> action, TimeSpan? expiry = null, TimeSpan? wait = null, TimeSpan? retry = null);

        Task<OperationInfo?> GetAsync(Guid id);

        Task RemoveAsync(Guid id);
    }
}
