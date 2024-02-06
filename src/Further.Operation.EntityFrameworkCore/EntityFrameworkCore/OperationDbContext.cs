using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Further.Operation.EntityFrameworkCore;

[ConnectionStringName(OperationDbProperties.ConnectionStringName)]
public class OperationDbContext : AbpDbContext<OperationDbContext>, IOperationDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public OperationDbContext(DbContextOptions<OperationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureOperation();
    }
}
