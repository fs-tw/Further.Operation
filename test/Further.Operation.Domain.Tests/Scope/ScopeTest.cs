using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Xunit;

namespace Further.Operation.Scope
{
    public class ScopeTest<TStartupModule> : OperationDomainTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IAmbientScopeProvider<List<string>> ambientScopeProvider;

        private const string TestScope = "TestScope";

        protected ScopeTest()
        {
            ambientScopeProvider = GetRequiredService<IAmbientScopeProvider<List<string>>>();
        }

        [Fact]
        public async Task MultiScopeGetValueAsync()
        {
            var scope1 = "";
            var scope2 = "";

            using (ambientScopeProvider.BeginScope(TestScope, new List<string> { "scope1" }))
            {
                using (ambientScopeProvider.BeginScope(TestScope, new List<string> { "scope2" }))
                {
                    scope2 = ambientScopeProvider.GetValue(TestScope)?.First();
                }

                scope1 = ambientScopeProvider.GetValue(TestScope)?.First();
            }

            Assert.Equal(scope1, "scope1");
            Assert.Equal(scope2, "scope2");
        }

        [Fact]
        public async Task MultiMethodGetValueAsync()
        {
            var result = new List<string>();

            using (ambientScopeProvider.BeginScope(TestScope, result))
            {
                Method1();

                await Method2();

                var currentData = ambientScopeProvider.GetValue(TestScope);

                Assert.NotNull(currentData);
                Assert.Equal(3, currentData.Count);
            }

            void Method1()
            {
                var currentData = ambientScopeProvider.GetValue(TestScope);

                currentData?.Add("Method1");
            }

            Task Method2()
            {
                var currentData = ambientScopeProvider.GetValue(TestScope);

                currentData?.Add("Method2");

                Method3();

                return Task.CompletedTask;
            }

            void Method3()
            {
                var currentData = ambientScopeProvider.GetValue(TestScope);

                currentData?.Add("Method3");
            }
        }

        [Fact]
        public async Task EnsureConcurrentAccessToScopeProviderBehavesCorrectly()
        {
            var tasks = new List<Task>();

            int numberOfTasks = 20;
            for (int i = 0; i < numberOfTasks; i++)
            {
                var value = new List<string> { i.ToString() };
                tasks.Add(Task.Run(() =>
                {
                    using (var scope = ambientScopeProvider.BeginScope(TestScope, value))
                    {
                        Task.Delay(10).Wait();

                        // 檢查設置後能否獲取到相同的值
                        var retrievedValue = ambientScopeProvider.GetValue(TestScope);
                        Assert.Equal(value.First(), retrievedValue.First());

                        var changed = $"changed{value.First()}";
                        retrievedValue[0] = changed;

                        // 再次延遲，然後檢查值是否仍然正確
                        Task.Delay(10).Wait();
                        retrievedValue = ambientScopeProvider.GetValue(TestScope);
                        Assert.Equal(changed, retrievedValue.First());
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }

        [Fact]
        public async Task MultiTherdGetAsync()
        {
            var result = new List<string>();

            using (ambientScopeProvider.BeginScope(TestScope, result))
            {
                var task1 = Task.Run(() =>
                {
                    var currentData = ambientScopeProvider.GetValue(TestScope);

                    currentData?.Add("Task1");
                });

                var task2 = Task.Run(async () =>
                {
                    var currentData = ambientScopeProvider.GetValue(TestScope);

                    currentData.RemoveAll(x => true);

                    currentData?.Add("Task2");

                    await Task.Delay(100);

                    var currentData2 = ambientScopeProvider.GetValue(TestScope);

                    currentData2?.Add("Task2-2");
                });

                await Task.WhenAll(task1, task2);

                var currentData = ambientScopeProvider.GetValue(TestScope);

                Assert.NotNull(currentData);
                Assert.Equal(2, currentData.Count);
            }
        }
    }
}
