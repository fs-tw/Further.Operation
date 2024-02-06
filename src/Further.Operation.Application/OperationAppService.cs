using Further.Operation.Localization;
using Volo.Abp.Application.Services;

namespace Further.Operation;

public abstract class OperationAppService : ApplicationService
{
    protected OperationAppService()
    {
        LocalizationResource = typeof(OperationResource);
        ObjectMapperContext = typeof(OperationApplicationModule);
    }
}
