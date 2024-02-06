using Further.Operation.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Further.Operation;

public abstract class OperationController : AbpControllerBase
{
    protected OperationController()
    {
        LocalizationResource = typeof(OperationResource);
    }
}
