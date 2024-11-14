using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.EntityFrameworkCore;
using Further.Operation.Data;
using Volo.Abp.AspNetCore.Mvc;

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
    }
}
