using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Further.Abp.Operation
{
    public interface IOperationProvider
    {
        Guid? CurrentId { get; }
        void Initialize(Guid? id = null);

        Task CreateOperationAsync(Guid id, Action<OperationInfo> action,TimeSpan? maxSurvivalTime = null);

        Task UpdateOperationAsync(Guid id, Action<OperationInfo> action, TimeSpan? maxSurvivalTime = null);

        Task<List<Guid>> GetListOperationIdAsync();

        Task<OperationInfo?> GetAsync(Guid id);

        Task RemoveAsync(Guid id);
    }
}
