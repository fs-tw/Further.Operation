using Localization.Resources.AbpUi;
using Further.Operation.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Further.Operation;

[DependsOn(
    typeof(OperationApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class OperationHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(OperationHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<OperationResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
