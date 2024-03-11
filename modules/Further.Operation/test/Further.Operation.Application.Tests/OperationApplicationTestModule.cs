using Volo.Abp.Modularity;

namespace Further.Operation;

[DependsOn(
    typeof(OperationApplicationModule),
    typeof(OperationDomainTestModule)
    )]
public class OperationApplicationTestModule : AbpModule
{

}
