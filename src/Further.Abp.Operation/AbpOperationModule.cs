using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

namespace Further.Abp.Operation;

[DependsOn(
    typeof(AbpCachingStackExchangeRedisModule))]
[DependsOn(
    typeof(Further.Abp.Operation.AbpOperationAbstractionsModule))]
public class AbpOperationModule : AbpModule
{
    //public override void PreConfigureServices(ServiceConfigurationContext context)
    //{
    //    context.Services.OnRegistered(OperationInterceptorRegistrar.RegisterIfNeeded);
    //}

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var redisService = context.ServiceProvider.GetRequiredService<RedisConnectorHelper>();
        redisService.Initialize();
        redisService.Subscribe();
    }
}
