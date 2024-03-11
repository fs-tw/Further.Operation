using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Further.Operation.EntityFrameworkCore;

public class OperationHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<OperationHttpApiHostMigrationsDbContext>
{
    public OperationHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<OperationHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Operation"));

        return new OperationHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
