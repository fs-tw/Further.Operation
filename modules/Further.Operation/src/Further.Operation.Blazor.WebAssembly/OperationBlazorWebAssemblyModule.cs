using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Further.Operation.Blazor.WebAssembly;

[DependsOn(
    typeof(OperationBlazorModule),
    typeof(OperationHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
)]
public class OperationBlazorWebAssemblyModule : AbpModule
{

}
