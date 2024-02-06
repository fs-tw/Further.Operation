using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Further.Operation.Seed;

/* You can use this file to seed some sample data
 * to test your module easier.
 *
 * This class is shared among these projects:
 * - Further.Operation.AuthServer
 * - Further.Operation.Web.Unified (used as linked file)
 */
public class OperationSampleDataSeeder : ITransientDependency
{
    public async Task SeedAsync(DataSeedContext context)
    {

    }
}
