using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Json;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Modularity;

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

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var redisService = context.ServiceProvider.GetRequiredService<RedisConnectorHelper>();
        redisService.Initialize();
        redisService.Subscribe();
        redisService.ReSubscribe();
    }
}
