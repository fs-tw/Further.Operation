using Further.Abp.Operation;
using Further.Operation.Operations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

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
        context.UseOperationCacheExpriedSave();
    }
}
