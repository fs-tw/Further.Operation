### 操作紀錄模組介紹

操作紀錄模組是一個用來記錄使用者操作的模組，它可以記錄使用者的操作紀錄，而且可以進行查詢。
並且利用redis過期事件的特性進行保存

### 操作紀錄模組功能

- 記錄使用者操作紀錄
- 查詢使用者操作紀錄

### 操作紀錄模組使用

實體介紹

```c#
public class OperationInfo
{
    public Guid Id { get; private set; }

    //操作function Name
    public string? OperationId { get; set; }

    //操作名稱
    public string? OperationName { get; set; }

    //過程每個步驟的訊息
    public Result Result { get; } = Result.Ok();

    //整個操作是否完成
    public bool IsSuccess => this.Result.IsSuccess;

    //操作所關聯的實體
    public List<OperationOwnerInfo> Owners { get; } = new();
}
```

使用方式

注入IOperationProvider介面，並且使用CreateOperationAsync方法進行操作紀錄的新增。

```c#
public class OperationSave : ITransientDependency
{
    protected IOperationProvider operationProvider { get; }

    protected OperationSaveTest(IOperationProvider operationProvider)
    {
        this.operationProvider = operationProvider;
    }

    public async Task OperationSaveAsync()
    {
        var operationId = Guid.NewGuid();

        await operationProvider.CreateOperationAsync(operationId, operationInfo =>
        {
            operationInfo.OperationId = operationId.ToString();
            operationInfo.OperationName = "TestOperation";
            operationInfo.Result.WithSuccess(new Success("OperationStore保存成功"));
            operationInfo.Owners.Add(new OperationOwnerInfo
            {
                EntityType = "TestOperationType",
                EntityId = Guid.NewGuid()
            });
        });
    }
}
```
假設你已經新增過操作紀錄，你可以在之前的操作紀錄上做訊息的追加，使用UpdateOperationAsync方法進行操作紀錄的更新。
這樣的設計避免了物件在方法間的傳遞，並且provider記住了currentId，所以不需要再次傳遞Id也能編輯同個物件

```c#
public class OperationSave : ITransientDependency
{
    protected IOperationProvider operationProvider { get; }

    protected OperationSaveTest(IOperationProvider operationProvider)
    {
        this.operationProvider = operationProvider;
    }

    public async Task OperationSaveAsync()
    {
        var operationId = operationProvider.GetCurrentId();

        await operationProvider.UpdateOperationAsync(operationId, operationInfo =>
        {
            operationInfo.OperationId = operationId.ToString();
            operationInfo.OperationName = "TestOperation";
            operationInfo.Result.WithSuccess(new Success("OperationStore保存成功"));
            operationInfo.Owners.Add(new OperationOwnerInfo
            {
                EntityType = "TestOperationType",
                EntityId = Guid.NewGuid()
            });
        });
    }
}
```

目前預設過期時間是5秒，但可在CreateOperationAsync和UpdateOperationAsync方法中設定過期時間。
過期後模駔會自動保存到資料庫中

```c#
Task CreateOperationAsync(Guid id, Action<OperationInfo> action, TimeSpan? slidingExpiration = null);

Task UpdateOperationAsync(Guid id, Action<OperationInfo> action, TimeSpan? slidingExpiration = null);
```