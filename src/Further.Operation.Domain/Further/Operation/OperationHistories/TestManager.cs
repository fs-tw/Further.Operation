using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Further.Operation.OperationHistories
{
    public class TestManager : ITransientDependency
    {
        public TestManager() { }

        [OperationHistory(EntityType = "Test")]
        public virtual string Test()
        {
            var test = Test2(Guid.NewGuid(), "test");

            return "Test";
        }

        [OperationHistory(EntityType = "Test2")]
        public virtual string Test2(Guid id,string test)
        {
            return "Test2";
        }
    }
}
