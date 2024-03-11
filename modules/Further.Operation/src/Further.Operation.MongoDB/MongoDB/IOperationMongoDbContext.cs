using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Further.Operation.MongoDB;

[ConnectionStringName(OperationDbProperties.ConnectionStringName)]
public interface IOperationMongoDbContext : IAbpMongoDbContext
{
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */
}
