using Localization.Resources.AbpUi;
using Further.Operation.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;

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

        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers
            .Create(typeof(Further.Operation.Operations.OperationController).Assembly,
            option =>
            {
                option.RootPath = OperationRemoteServiceConsts.ModuleName;
                option.RemoteServiceName = OperationRemoteServiceConsts.RemoteServiceName;
                option.ApplicationServiceTypes = ApplicationServiceTypes.All;
            });
        });
    }
}
