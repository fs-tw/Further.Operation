using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Further.Operation.Localization;
using Volo.Abp.Domain;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Further.Operation;

[DependsOn(
    typeof(OperationDomainModule)
)]
public class OperationEventHandlerModule : AbpModule
{
}
