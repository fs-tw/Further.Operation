using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Further.Operation.Blazor.Server.Host;

[Dependency(ReplaceServices = true)]
public class OperationBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Operation";
}
