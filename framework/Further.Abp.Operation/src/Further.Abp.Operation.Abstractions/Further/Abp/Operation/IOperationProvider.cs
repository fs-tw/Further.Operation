using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Further.Abp.Operation
{
    public interface IOperationProvider
    {
        Guid? GetCurrentId();

        void SetCurrentId(Guid id);

        Task InitializeAsync();

        Task CreateOperationAsync(Guid id, Action<OperationInfo> action, TimeSpan? slidingExpiration = null);

        Task UpdateOperationAsync(Guid id, Action<OperationInfo> action, TimeSpan? slidingExpiration = null);

        Task<List<Guid>> ListIdsAsync();

        Task<OperationInfo?> GetAsync(Guid id);

        Task RemoveAsync(Guid id);
    }
}
