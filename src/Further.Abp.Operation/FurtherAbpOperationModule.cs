using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Further.Abp.Operation;


public class FurtherAbpOperationModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(OperationInterceptorRegistrar.RegisterIfNeeded);
    }
}
