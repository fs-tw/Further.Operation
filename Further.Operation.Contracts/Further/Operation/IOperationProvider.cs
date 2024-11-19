using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Further.Operation
{
    public interface IOperationProvider
    {
        Task CreateAsync(Guid id, OperationInfo operationInfo, TimeSpan? slidingExpiration = null, Action<OperationInfo>? errorAction = null);

        Task UpdateAsync(Guid id, Action<OperationInfo> action, TimeSpan? slidingExpiration = null, Action<OperationInfo>? errorAction = null);

        Task FinishAsync(Guid id, Action<OperationInfo>? errorAction = null);

        Task<List<OperationInfo>> GetListAsync();

        Task<OperationInfo?> GetAsync(Guid id);

        //暫不開放Remove
        //Task RemoveAsync(Guid id);
    }
}
