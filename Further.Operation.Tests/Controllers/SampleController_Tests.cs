using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Further.Operation.Samples;

namespace Further.Operation.Tests.Controllers;

/* This is a demo test class to show how to test HTTP API controllers.
 * You can delete this class freely.
 *
 * See https://docs.abp.io/en/abp/latest/Testing for more about automated tests.
 */

public class SampleController_Tests : OperationIntegrationTestBase
{
    [Fact]
    public async Task GetAsync()
    {
        var response = await GetResponseAsObjectAsync<SampleDto>("api/Operation/sample");
        response.Value.ShouldBe(42);
    }
}