using Further.Abp.Operation;
using Further.Operation.Operations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Further.Operation;

[DependsOn(
    typeof(AbpCachingStackExchangeRedisModule)
    )]
[DependsOn(
       typeof(OperationDomainModule)
    )]
public class OperationRedisModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var redisService = context.ServiceProvider.GetRequiredService<RedisSubscriptionService>();
        redisService.Initialize();
    }
}
