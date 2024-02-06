using Further.Operation.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Further.Operation.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class OperationPageModel : AbpPageModel
{
    protected OperationPageModel()
    {
        LocalizationResourceType = typeof(OperationResource);
        ObjectMapperContext = typeof(OperationWebModule);
    }
}
