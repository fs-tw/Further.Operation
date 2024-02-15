using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Further.Abp.Operation
{
    public class SimpleOperationStore : IOperationStore, ITransientDependency
    {
        //此為範例請自行實作Store保存資料
        public Task SaveAsync(OperationInfo? operationInfo, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
