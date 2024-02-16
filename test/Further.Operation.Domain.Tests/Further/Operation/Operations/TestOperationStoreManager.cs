using Further.Abp.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Further.Operation.Operations
{
    [OperationScope("TestOperationStoreManager", "測試OperationStore")]
    public class TestOperationStoreManager : ITransientDependency
    {
        private readonly IOperationScopeProvider operationScopeProvider;

        public TestOperationStoreManager(
            IOperationScopeProvider operationScopeProvider)
        {
            this.operationScopeProvider = operationScopeProvider;
        }

        [OperationMessage("OperationStore保存成功")]
        public virtual Task<Guid> SaveAsync()
        {
            return Task.FromResult(operationScopeProvider.Current.Id);
        }
    }
}
