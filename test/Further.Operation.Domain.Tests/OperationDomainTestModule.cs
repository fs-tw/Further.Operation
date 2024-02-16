using Further.Operation.Options;
using Further.Operation.Options.TypeDefinitions;
using Volo.Abp.Modularity;

namespace Further.Operation;

[DependsOn(
    typeof(OperationDomainModule),
    typeof(OperationTestBaseModule)
)]
public class OperationDomainTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<FurtherOperationOptions>(options =>
        {
            options.EntityTypes.Add(new OperationOwnerTypeDefinition("TestOperationType"));
        });
    }
}
