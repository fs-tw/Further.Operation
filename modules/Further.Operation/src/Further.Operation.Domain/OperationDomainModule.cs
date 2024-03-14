using Further.Abp.Operation;
using Further.Operation.Operations;
using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Further.Operation;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpCachingModule),
    typeof(OperationDomainSharedModule)
)]
[DependsOn(typeof(Further.Abp.Operation.AbpOperationAbstractionsModule))]
public class OperationDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}
