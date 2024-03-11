using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Further.Operation.Options.TypeDefinitions
{
    public class OperationOwnerTypeDefinitionStore : IOperationOwnerTypeDefinitionStore, ITransientDependency
    {
        private FurtherOperationOptions options;

        public OperationOwnerTypeDefinitionStore(
            IOptions<FurtherOperationOptions> options)
        {
            this.options = options.Value;
        }

        public Task<OperationOwnerTypeDefinition> GetAsync(string entityType)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

            var definition = options.EntityTypes.SingleOrDefault(x => x.EntityType.Equals(entityType,
                StringComparison.InvariantCultureIgnoreCase)) ?? throw new UserFriendlyException("找不到EntityType");

            return Task.FromResult(definition);
        }

        public Task<bool> IsDefinedAsync(string entityType)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

            var isDefined = options.EntityTypes.Any(x => x.EntityType.Equals(entityType, StringComparison.InvariantCultureIgnoreCase));

            return Task.FromResult(isDefined);
        }

        public async Task VaildEntityTypeAsync(string entityType)
        {
            if (!await this.IsDefinedAsync(entityType))
            {
                throw new UserFriendlyException("找不到EntityType");
            }
        }
    }
}
