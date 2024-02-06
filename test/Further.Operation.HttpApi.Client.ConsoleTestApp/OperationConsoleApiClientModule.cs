using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Further.Operation;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(OperationHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class OperationConsoleApiClientModule : AbpModule
{

}
