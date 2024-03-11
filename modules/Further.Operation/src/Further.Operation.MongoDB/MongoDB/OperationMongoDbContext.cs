using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Further.Operation.MongoDB;

[ConnectionStringName(OperationDbProperties.ConnectionStringName)]
public class OperationMongoDbContext : AbpMongoDbContext, IOperationMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureOperation();
    }
}
