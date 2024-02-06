using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Further.Operation;

[DependsOn(
    typeof(OperationDomainModule),
    typeof(OperationApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class OperationApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<OperationApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<OperationApplicationModule>(validate: true);
        });
    }
}
