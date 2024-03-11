using Volo.Abp.Modularity;

namespace Further.Operation;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class OperationApplicationTestBase<TStartupModule> : OperationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
