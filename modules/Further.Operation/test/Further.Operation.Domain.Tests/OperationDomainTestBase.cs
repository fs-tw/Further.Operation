using Volo.Abp.Modularity;

namespace Further.Operation;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class OperationDomainTestBase<TStartupModule> : OperationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
