using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Further.Abp.Operation
{
    public class TestOperationManager : ITransientDependency
    {
        private readonly IOperationProvider operationProvider;

        public TestOperationManager(IOperationProvider operationProvider)
        {
            this.operationProvider = operationProvider;
        }

        public async Task<Guid> GetCurrentId()
        {
            return (Guid)operationProvider.CurrentId;
        }
    }

    public class TestOperationManager2 : ITransientDependency
    {
        private readonly IOperationProvider operationProvider;

        public TestOperationManager2(IOperationProvider operationProvider)
        {
            this.operationProvider = operationProvider;
        }

        public async Task<Guid> GetCurrentId()
        {
            return (Guid)operationProvider.CurrentId;
        }
    }

    public class TestOperationManager3 : ITransientDependency
    {
        private readonly IOperationProvider operationProvider;

        public TestOperationManager3(IOperationProvider operationProvider)
        {
            this.operationProvider = operationProvider;
        }

        public async Task<Guid> GetCurrentId()
        {
            operationProvider.Initialize();
            return (Guid)operationProvider.CurrentId;
        }
    }

    public class TestOperationManager4 : ITransientDependency
    {
        private readonly TestOperationManager2 testOperationManager2;
        private readonly IOperationProvider operationProvider;

        public TestOperationManager4(
            TestOperationManager2 testOperationManager2,
            IOperationProvider operationProvider)
        {
            this.testOperationManager2 = testOperationManager2;
            this.operationProvider = operationProvider;
        }

        public async Task<bool> CheckCurrentId()
        {
            return operationProvider.CurrentId == await testOperationManager2.GetCurrentId();
        }
    }
}
