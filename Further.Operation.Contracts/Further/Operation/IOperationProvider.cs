using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Further.Operation
{
    public interface IOperationProvider
    {
        Task CreateOperationAsync(Guid id, Action<OperationInfo> action, TimeSpan? slidingExpiration = null);

        Task UpdateOperationAsync(Guid id, Action<OperationInfo> action, TimeSpan? slidingExpiration = null);

        Task FinishOperationAsync(Guid id);

        Task<List<Guid>> ListIdsAsync();

        Task<OperationInfo?> GetAsync(Guid id);

        Task RemoveAsync(Guid id);
    }
}
