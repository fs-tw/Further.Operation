using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Further.Abp.Operation;

[DependsOn(
    typeof(AbpCachingStackExchangeRedisModule))]
[DependsOn(
    typeof(Further.Abp.Operation.AbpOperationAbstractionsModule))]
public class AbpOperationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<OperationOptions>(configuration.GetSection("Operation"));

        Configure<AbpSystemTextJsonSerializerOptions>(x =>
        {
            x.JsonSerializerOptions.Converters.Add(new ResultConverter());
            //x.JsonSerializerOptions
        });

        //Configure<AbpSystemTextJsonSerializerModifiersOptions>(x => 
        //{
        //    x.Modifiers.Add(y=>y.)
        //});

        //Configure<AbpNewtonsoftJsonSerializerOptions>(options =>
        //{
        //    //options.JsonSerializerSettings.Converters.Add(new FluentResultConverter());
        //    //options.JsonSerializerSettings.Converters.Add(new OperationInfoConverter());
        //});
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider.GetRequiredService<IOperationProvider>().InitializeAsync();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }
}
