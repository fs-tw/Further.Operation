using Further.Abp.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Further.Operation.Operations
{
    public class TestOperationOwner
    {
        public Guid Id { get; set; }

        public Guid OpenInfoId { get; set; }
    }

    [OperationScope("TestOperationStoreManager", "測試OperationStore",MaxSurvivalTime = 3)]
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

        [OperationOwner(typeof(TestOperationOwner))]
        public virtual Task<TestOperationOwner> SaveOwnerAsync()
        {
            var test = new TestOperationOwner
            {
                Id = Guid.NewGuid(),
                OpenInfoId = operationScopeProvider.Current.Id
            };

            return Task.FromResult(test);
        }
    }
}
