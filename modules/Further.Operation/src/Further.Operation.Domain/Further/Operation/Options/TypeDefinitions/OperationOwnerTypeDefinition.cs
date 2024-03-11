using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Operation.Options.TypeDefinitions
{
    public class OperationOwnerTypeDefinition
    {
        public OperationOwnerTypeDefinition(string entityType)
        {
            this.EntityType = entityType;
        }

        public string EntityType { get; protected set; }
    }
}
