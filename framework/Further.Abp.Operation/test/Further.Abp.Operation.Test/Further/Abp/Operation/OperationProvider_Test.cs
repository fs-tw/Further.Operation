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
    public class OperationProvider_Test : OperationTestBase
    {
        private readonly IOperationProvider operationProvider;

        public OperationProvider_Test()
        {
            operationProvider = GetRequiredService<IOperationProvider>();
        }

        [Fact]
        public async Task CreateOperationAsync()
        {
            var operationId = Guid.NewGuid();
            var message = "Test";

            // 定義修改操作
            Action<OperationInfo> action = (op) =>
            {
                op.GetResult().WithSuccess(new Success(message));
            };

            // 執行修改操作
            await operationProvider.CreateOperationAsync(operationId, action);

            // 驗證
            var finalOperationInfo = await operationProvider.GetAsync(operationId);
            Assert.Equal(message, finalOperationInfo?.Result.Reasons.First().Message);
        }

        [Fact]
        public async Task UpdateOperationAsync_MultipleThreads()
        {
            var operationId = Guid.NewGuid();
            var random = new Random();

            await operationProvider.CreateOperationAsync(operationId, o =>
            {
                o.OperationId = "UpdateOperationAsync_MultipleThreads";
            });

            // 定義修改操作
            Action<OperationInfo> action = (op) =>
            {
                op.GetResult().WithSuccess(new Success("Test"));
            };

            int numberOfTasks = 10; // 並行任務的數量
            var tasks = new List<Task>();

            for (int i = 0; i < numberOfTasks; i++)
            {
                // 每個任務都調用 CreateOperationAsync
                tasks.Add(Task.Run(async () =>
                {
                    var delay = random.Next(50, 151);
                    await Task.Delay(delay);
                    await operationProvider.UpdateOperationAsync(operationId, action);
                }));
            }

            // 等待所有任務完成
            await Task.WhenAll(tasks);

            // 驗證
            var finalOperationInfo = await operationProvider.GetAsync(operationId);
            Assert.Equal(numberOfTasks, finalOperationInfo?.Result.Reasons.Count);
        }

        [Fact]
        public async Task UpdateOperationAsync_MultipleThreads_NotOverAdd()
        {
            var operationId = Guid.NewGuid();
            var maxNumberOfReasons = 10;
            var random = new Random();

            await operationProvider.CreateOperationAsync(operationId, o =>
            {
                o.OperationId = "UpdateOperationAsync_MultipleThreads_NotOverAdd";
            });

            // 定義修改操作
            Action<OperationInfo> action = (op) =>
            {
                if (op.Result.Reasons.Count >= maxNumberOfReasons)
                {
                    return;
                }
                op.GetResult().WithSuccess(new Success("Test"));
            };

            int numberOfTasks = 50; // 並行任務的數量
            var tasks = new List<Task>();

            for (int i = 0; i < numberOfTasks; i++)
            {
                // 每個任務都調用 CreateOperationAsync
                tasks.Add(Task.Run(async () =>
                {
                    var delay = random.Next(50, 151);
                    await Task.Delay(delay);
                    await operationProvider.UpdateOperationAsync(operationId, action);
                }));
            }

            // 等待所有任務完成
            await Task.WhenAll(tasks);

            // 驗證
            var finalOperationInfo = await operationProvider.GetAsync(operationId);
            Assert.Equal(maxNumberOfReasons, finalOperationInfo?.Result.Reasons.Count);
        }

        [Fact]
        public async Task UpdateOperationAsync_MultipleThreads_NotOverAdd_WithCurrentId()
        {
            operationProvider.SetCurrentId(Guid.NewGuid());
            var operationId = operationProvider.GetCurrentId();
            var maxNumberOfReasons = 10;
            var random = new Random();

            await operationProvider.CreateOperationAsync((Guid)operationId, o =>
            {
                o.OperationId = "UpdateOperationAsync_MultipleThreads_NotOverAdd_WithCurrentId";
            });

            // 定義修改操作
            Action<OperationInfo> action = (op) =>
            {
                if (op.Result.Reasons.Count >= maxNumberOfReasons)
                {
                    return;
                }
                op.GetResult().WithSuccess(new Success("Test"));
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
                    await operationProvider.UpdateOperationAsync((Guid)operationId, action);
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
            var manager1 = GetRequiredService<TestOperationManager>();
            var manager2 = GetRequiredService<TestOperationManager2>();

            operationProvider.SetCurrentId(Guid.NewGuid());

            var id1 = await manager1.GetCurrentId();
            var id2 = await manager2.GetCurrentId();

            Assert.Equal(id1, id2);
        }

        [Fact]
        public async Task CurrentIdTestAsync_WithInitialize()
        {
            var manager1 = GetRequiredService<TestOperationManager>();
            var manager3 = GetRequiredService<TestOperationManager3>();

            operationProvider.SetCurrentId(Guid.NewGuid());

            var id1 = await manager1.GetCurrentId();
            var id2 = await manager3.GetCurrentId();

            Assert.Equal(operationProvider.GetCurrentId(), id1);
            Assert.NotEqual(operationProvider.GetCurrentId(), id2);
        }

        [Fact]
        public async Task CurrentIdTestAsync_WithInitialize_WithDependency()
        {
            var manager2 = GetRequiredService<TestOperationManager2>();
            var manager4 = GetRequiredService<TestOperationManager4>();

            operationProvider.SetCurrentId(Guid.NewGuid());

            var id1 = await manager2.GetCurrentId();
            var id2 = await manager4.CheckCurrentId();

            Assert.Equal(operationProvider.GetCurrentId(), id1);
            Assert.True(id2);
        }

        [Fact]
        public async Task OperationGetListIdAsync()
        {
            var operationId = Guid.NewGuid();

            await operationProvider.CreateOperationAsync(operationId, operationInfo =>
            {
                operationInfo.OperationId = operationId.ToString();
            });

            var ids = await operationProvider.ListIdsAsync();

            Assert.NotNull(ids);
            Assert.Contains(operationId, ids);
        }
    }
}
