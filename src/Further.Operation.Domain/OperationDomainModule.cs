using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Further.Operation;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpCachingModule),
    typeof(OperationDomainSharedModule)
)]
public class OperationDomainModule : AbpModule
{

}
