using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Further.Operation.Data;

[ConnectionStringName(OperationDbProperties.ConnectionStringName)]
public interface IOperationDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
