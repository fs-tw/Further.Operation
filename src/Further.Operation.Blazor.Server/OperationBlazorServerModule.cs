using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace Further.Operation.Blazor.Server;

[DependsOn(
    typeof(OperationBlazorModule),
    typeof(AbpAspNetCoreComponentsServerThemingModule)
    )]
public class OperationBlazorServerModule : AbpModule
{

}
