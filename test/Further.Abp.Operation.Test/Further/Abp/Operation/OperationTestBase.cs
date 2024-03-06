using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Testing;

namespace Further.Abp.Operation
{
    public class OperationTestBase : AbpIntegratedTest<FurtherAbpOperationTestBaseModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        protected override void BeforeAddApplication(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", false);
            builder.AddJsonFile("appsettings.secrets.json", true);
            services.ReplaceConfiguration(builder.Build());
        }
    }
}
