using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.EntityFrameworkCore;
using Further.Operation.Data;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Json.SystemTextJson;

namespace Further.Operation;

[DependsOn(
    typeof(OperationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class OperationModule : AbpModule
{
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
            //x.JsonSerializerOptions
        });
    }
}
