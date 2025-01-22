using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.EntityFrameworkCore;
using Further.Operation.Data;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Json.SystemTextJson;
using System.Threading.Tasks;
using Volo.Abp.Threading;
using Volo.Abp;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Volo.Abp.Caching.StackExchangeRedis;
using StackExchange.Redis;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System;
using Volo.Abp.Caching;
using Volo.Abp.EventBus.Distributed;

namespace Further.Operation;

[DependsOn(
    typeof(OperationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpEntityFrameworkCoreModule),
    typeof(AbpCachingStackExchangeRedisModule)
)]
public class OperationModule : AbpModule
{
    private readonly RedisCacheOptions options;


    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<OperationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<OperationModule>(validate: true);
        });

        context.Services.AddAbpDbContext<OperationDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
        });

        context.Services.AddSingleton<ICurrentOperationAccessor>(CurrentOperationAccessor.Instance);

        Configure<AbpSystemTextJsonSerializerOptions>(x =>
        {
            x.JsonSerializerOptions.Converters.Add(new ResultConverter());
        });
        var configuration = context.Services.GetConfiguration();
        var redisEnabled = configuration["Redis:IsEnabled"];

        Configure<RedisCacheOptions>(o =>
        {
            if (!(string.IsNullOrEmpty(redisEnabled) || bool.Parse(redisEnabled)))
            {
                return;
            }

            if (!o.Configuration.IsNullOrEmpty())
            {
                o.ConnectionMultiplexerFactory = async () =>
                {
                    var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(o.Configuration);
                    await SubscribeKeyExpiredOnceAsync(context, connectionMultiplexer);
                    return connectionMultiplexer;
                };
            }
        });
    }

    private async Task SubscribeKeyExpiredOnceAsync(ServiceConfigurationContext context, ConnectionMultiplexer connectionMultiplexer)
    {
        var subscriber = connectionMultiplexer!.GetSubscriber();

        connectionMultiplexer.GetServer(connectionMultiplexer.GetEndPoints().Single())
            .ConfigSet("notify-keyspace-events", "KEA");

        await subscriber.UnsubscribeAsync("__keyevent@0__:expired");

        await subscriber.SubscribeAsync("__keyevent@0__:expired", async (channel, value) =>
        {

            if (!value.ToString().Contains($"k:{OperationConsts.PrrfixOperationIdKey}")) return;

            var (cValue, kValue) = ParseRedisKey(value.ToString());
            Guid operationId = Guid.NewGuid();
            if (Guid.TryParse(kValue.Replace($"{OperationConsts.PrrfixOperationIdKey}_", ""), out operationId))
            {
                var distributedCache = context.Services.GetRequiredService<IDistributedCache<BasicOperationInfo>>();
                var distributedEventBus = context.Services.GetRequiredService<IDistributedEventBus>();
                var operationInfo = await distributedCache.GetAsync(OperationConsts.GetOperationValueKey(operationId));

                if (operationInfo == null) return;

                await distributedCache.RemoveAsync(OperationConsts.GetOperationValueKey(operationId));

                await distributedEventBus.PublishAsync(new OperationExpiredEto
                {
                    OperationInfo = operationInfo
                });
            }
        });
    }
    //c:Further.Operation.BasicOperationInfo,k:OperationId_7fad5c93-637b-4b99-89c3-1b106a28f94f
    private (string cValue, string kValue) ParseRedisKey(string input)
    {
        var parts = input.Split(',');
        var cValue = parts.FirstOrDefault(p => p.StartsWith("c:"))?.Substring(2);
        var kValue = parts.FirstOrDefault(p => p.StartsWith("k:"))?.Substring(2);
        return (cValue, kValue);
    }

}
