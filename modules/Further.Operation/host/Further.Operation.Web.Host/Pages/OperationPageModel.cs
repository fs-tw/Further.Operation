using Further.Operation.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Further.Operation.Pages;

public abstract class OperationPageModel : AbpPageModel
{
    protected OperationPageModel()
    {
        LocalizationResourceType = typeof(OperationResource);
    }
}
