using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Json;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Further.Abp.Operation;

[DependsOn(
    typeof(AbpJsonNewtonsoftModule),
    typeof(AbpCachingStackExchangeRedisModule))]
[DependsOn(
    typeof(Further.Abp.Operation.AbpOperationAbstractionsModule))]
public class AbpOperationModule : AbpModule
{
    //public override void PreConfigureServices(ServiceConfigurationContext context)
    //{
    //    context.Services.OnRegistered(OperationInterceptorRegistrar.RegisterIfNeeded);
    //}

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<OperationOptions>(configuration.GetSection("Operation"));

        Configure<AbpNewtonsoftJsonSerializerOptions>(options =>
        {
            options.JsonSerializerSettings.Converters.Add(new FluentResultConverter());
            options.JsonSerializerSettings.Converters.Add(new OperationInfoConverter());
        });
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var operationProvider = context.ServiceProvider.GetRequiredService<OperationProvider>();
        await operationProvider.InitializeAsync();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }
}
