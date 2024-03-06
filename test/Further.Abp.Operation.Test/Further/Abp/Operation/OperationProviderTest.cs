using FluentResults;
using RedLockNet.SERedis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Further.Abp.Operation
{
    public class OperationProviderTest : OperationTestBase
    {
        private readonly IOperationProvider operationProvider;

        public OperationProviderTest()
        {
            operationProvider = GetRequiredService<IOperationProvider>();
        }

        [Fact]
        public async Task ModifyOperationAsync()
        {
            var operationId = Guid.NewGuid();
            var message = "Test";

            // 定義修改操作
            Action<OperationInfo> action = (op) =>
            {
                op.Result.WithSuccess(new Success(message));
            };

            // 執行修改操作
            await operationProvider.ModifyOperationAsync(operationId, action);

            // 驗證
            var finalOperationInfo = await operationProvider.GetAsync(operationId);
            Assert.Equal(message, finalOperationInfo?.Result.Reasons.First().Message);
        }

        [Fact]
        public async Task ModifyOperationAsync_MultipleThreads()
        {
            var operationId = Guid.NewGuid();
            var random = new Random();

            // 定義修改操作
            Action<OperationInfo> action = (op) =>
            {
                op.Result.WithSuccess(new Success("Test"));
            };

            int numberOfTasks = 10; // 並行任務的數量
            var tasks = new List<Task>();

            for (int i = 0; i < numberOfTasks; i++)
            {
                // 每個任務都調用 ModifyOperationAsync
                tasks.Add(Task.Run(async () =>
                {
                    var delay = random.Next(50, 151);
                    await Task.Delay(delay);
                    await operationProvider.ModifyOperationAsync(operationId, action);
                }));
            }

            // 等待所有任務完成
            await Task.WhenAll(tasks);

            // 驗證
            var finalOperationInfo = await operationProvider.GetAsync(operationId);
            Assert.Equal(numberOfTasks, finalOperationInfo?.Result.Reasons.Count);
        }

        [Fact]
        public async Task ModifyOperationAsync_MultipleThreads_NotOverAdd()
        {
            var operationId = Guid.NewGuid();
            var maxNumberOfReasons = 10;
            var random = new Random();

            // 定義修改操作
            Action<OperationInfo> action = (op) =>
            {
                if (op.Result.Reasons.Count >= maxNumberOfReasons)
                {
                    return;
                }
                op.Result.WithSuccess(new Success("Test"));
            };

            int numberOfTasks = 50; // 並行任務的數量
            var tasks = new List<Task>();

            for (int i = 0; i < numberOfTasks; i++)
            {
                // 每個任務都調用 ModifyOperationAsync
                tasks.Add(Task.Run(async () =>
                {
                    var delay = random.Next(50, 151);
                    await Task.Delay(delay);
                    await operationProvider.ModifyOperationAsync(operationId, action);
                }));
            }

            // 等待所有任務完成
            await Task.WhenAll(tasks);

            // 驗證
            var finalOperationInfo = await operationProvider.GetAsync(operationId);
            Assert.Equal(maxNumberOfReasons, finalOperationInfo?.Result.Reasons.Count);
        }

        [Fact]
        public async Task ModifyOperationAsync_MultipleThreads_NotOverAdd_WithCurrentId()
        {
            operationProvider.Initialize();
            var operationId = operationProvider.CurrentId;
            var maxNumberOfReasons = 10;
            var random = new Random();

            // 定義修改操作
            Action<OperationInfo> action = (op) =>
            {
                if (op.Result.Reasons.Count >= maxNumberOfReasons)
                {
                    return;
                }
                op.Result.WithSuccess(new Success("Test"));
            };

            int numberOfTasks = 50; // 並行任務的數量
            var tasks = new List<Task>();

            for (int i = 0; i < numberOfTasks; i++)
            {
                // 每個任務都調用 ModifyOperationAsync
                tasks.Add(Task.Run(async () =>
                {
                    var delay = random.Next(50, 151);
                    await Task.Delay(delay);
                    await operationProvider.ModifyOperationAsync(action);
                }));
            }

            // 等待所有任務完成
            await Task.WhenAll(tasks);

            // 驗證
            var finalOperationInfo = await operationProvider.GetAsync((Guid)operationId);
            Assert.Equal(maxNumberOfReasons, finalOperationInfo?.Result.Reasons.Count);
        }

        [Fact]
        public async Task CurrentIdTestAsync()
        {
            operationProvider.Initialize();
            var manager1 = GetRequiredService<TestOperationManager>();
            var manager2 = GetRequiredService<TestOperationManager2>();
            var id1 = await manager1.GetCurrentId();
            var id2 = await manager2.GetCurrentId();

            Assert.Equal(id1, id2);
        }

        [Fact]
        public async Task CurrentIdTestAsync_WithInitialize()
        {
            var manager1 = GetRequiredService<TestOperationManager>();
            var manager3 = GetRequiredService<TestOperationManager3>();
            operationProvider.Initialize();
            var id1 = await manager1.GetCurrentId();
            var id2 = await manager3.GetCurrentId();

            Assert.Equal(operationProvider.CurrentId, id1);
            Assert.NotEqual(operationProvider.CurrentId, id2);
        }

        [Fact]
        public async Task CurrentIdTestAsync_WithInitialize_WithDependency()
        {
            var manager2 = GetRequiredService<TestOperationManager2>();
            var manager4 = GetRequiredService<TestOperationManager4>();
            operationProvider.Initialize();
            var id1 = await manager2.GetCurrentId();
            var id2 = await manager4.CheckCurrentId();

            Assert.Equal(operationProvider.CurrentId, id1);
            Assert.True(id2);
        }
    }
}
