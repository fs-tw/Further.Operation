using Volo.Abp.Modularity;

namespace Further.Operation;

[DependsOn(
    typeof(OperationDomainModule),
    typeof(OperationTestBaseModule)
)]
public class OperationDomainTestModule : AbpModule
{

}
