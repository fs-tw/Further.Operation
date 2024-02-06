using Further.Operation.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Further.Operation.Blazor;

public abstract class OperationComponentBase : AbpComponentBase
{
    protected OperationComponentBase()
    {
        LocalizationResource = typeof(OperationResource);
    }
}
