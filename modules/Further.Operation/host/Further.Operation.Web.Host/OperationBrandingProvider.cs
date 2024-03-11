using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Further.Operation;

[Dependency(ReplaceServices = true)]
public class OperationBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Operation";
}
