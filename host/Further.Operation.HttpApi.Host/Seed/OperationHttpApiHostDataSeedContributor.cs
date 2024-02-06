using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Further.Operation.Seed;

public class OperationHttpApiHostDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly OperationSampleDataSeeder _operationSampleDataSeeder;
    private readonly ICurrentTenant _currentTenant;

    public OperationHttpApiHostDataSeedContributor(
        OperationSampleDataSeeder operationSampleDataSeeder,
        ICurrentTenant currentTenant)
    {
        _operationSampleDataSeeder = operationSampleDataSeeder;
        _currentTenant = currentTenant;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(context?.TenantId))
        {
            await _operationSampleDataSeeder.SeedAsync(context!);
        }
    }
}
