using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Further.Operation
{
    public class OperationExpiredResolver : ITransientDependency
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IOptions<OperationOptions> options;

        public OperationExpiredResolver(
            IServiceProvider serviceProvider,
            IOptions<OperationOptions> options)
        {
            this.serviceProvider = serviceProvider;
            this.options = options;
        }

        public async Task ApplyExpiredProvidersAsync(OperationInfo operationInfo)
        {
            foreach (var expiredProviderType in options.Value.ExpiredProviders)
            {
                var expiredProvider = (IOperationExpiredProvider)serviceProvider.GetRequiredService(expiredProviderType);
                await expiredProvider.ExecuteAsync(operationInfo);
            }
        }
    }
}
