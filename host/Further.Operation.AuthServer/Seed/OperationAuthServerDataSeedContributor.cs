using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Further.Operation.Seed;

public class OperationAuthServerDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly OperationSampleIdentityDataSeeder _operationSampleIdentityDataSeeder;
    private readonly OperationAuthServerDataSeeder _operationAuthServerDataSeeder;
    private readonly ICurrentTenant _currentTenant;

    public OperationAuthServerDataSeedContributor(
        OperationAuthServerDataSeeder operationAuthServerDataSeeder,
        OperationSampleIdentityDataSeeder operationSampleIdentityDataSeeder,
        ICurrentTenant currentTenant)
    {
        _operationAuthServerDataSeeder = operationAuthServerDataSeeder;
        _operationSampleIdentityDataSeeder = operationSampleIdentityDataSeeder;
        _currentTenant = currentTenant;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(context?.TenantId))
        {
            await _operationSampleIdentityDataSeeder.SeedAsync(context!);
            await _operationAuthServerDataSeeder.SeedAsync(context!);
        }
    }
}
