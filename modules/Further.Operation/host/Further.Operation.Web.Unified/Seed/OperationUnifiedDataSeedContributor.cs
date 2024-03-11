using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Further.Operation.Seed;

public class OperationUnifiedDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly OperationSampleIdentityDataSeeder _sampleIdentityDataSeeder;
    private readonly OperationSampleDataSeeder _operationSampleDataSeeder;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly ICurrentTenant _currentTenant;

    public OperationUnifiedDataSeedContributor(
        OperationSampleIdentityDataSeeder sampleIdentityDataSeeder,
        IUnitOfWorkManager unitOfWorkManager,
        OperationSampleDataSeeder operationSampleDataSeeder,
        ICurrentTenant currentTenant)
    {
        _sampleIdentityDataSeeder = sampleIdentityDataSeeder;
        _unitOfWorkManager = unitOfWorkManager;
        _operationSampleDataSeeder = operationSampleDataSeeder;
        _currentTenant = currentTenant;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await _unitOfWorkManager.Current!.SaveChangesAsync();

        using (_currentTenant.Change(context.TenantId))
        {
            await _sampleIdentityDataSeeder.SeedAsync(context);
            await _unitOfWorkManager.Current.SaveChangesAsync();
            await _operationSampleDataSeeder.SeedAsync(context);
        }
    }
}
