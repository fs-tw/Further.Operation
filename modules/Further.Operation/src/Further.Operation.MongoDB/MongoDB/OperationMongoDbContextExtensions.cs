using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Further.Operation.MongoDB;

public static class OperationMongoDbContextExtensions
{
    public static void ConfigureOperation(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
