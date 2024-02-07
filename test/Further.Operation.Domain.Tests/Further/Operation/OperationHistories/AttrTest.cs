using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace Further.Operation.OperationHistories
{
    public class AttrTest<TStartupModule> : OperationDomainTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly TestManager testManager;
        public AttrTest()
        {
            testManager = GetRequiredService<TestManager>();
        }

        [Fact]
        public async Task Test1()
        {
            var result = testManager.Test();

            //var result2 = testManager.Test2(Guid.Empty,"Test2");
        }
    }
}
