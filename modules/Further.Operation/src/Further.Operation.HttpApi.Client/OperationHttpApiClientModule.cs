using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Further.Operation;

[DependsOn(
    typeof(OperationApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class OperationHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(OperationApplicationContractsModule).Assembly,
            OperationRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<OperationHttpApiClientModule>();
        });
    }
}
