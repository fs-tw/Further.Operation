using Further.Operation.Tests;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("Further.Operation.csproj");
await builder.RunAbpModuleAsync<OperationTestsModule>(applicationName: "Further.Operation");

public partial class TestProgram
{
}
