using Further.Operation.Options.TypeDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Operation.Options
{
    public class FurtherOperationOptions
    {
        public List<OperationOwnerTypeDefinition> EntityTypes { get; } = new();
    }
}
