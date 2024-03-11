using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Further.Operation.Options.TypeDefinitions
{
    public interface IOperationOwnerTypeDefinitionStore
    {
        Task<OperationOwnerTypeDefinition> GetAsync(string entityType);

        Task<bool> IsDefinedAsync(string entityType);

        Task VaildEntityTypeAsync(string entityType);
    }
}
