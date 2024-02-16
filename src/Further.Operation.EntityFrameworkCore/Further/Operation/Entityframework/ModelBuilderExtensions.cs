using Microsoft.EntityFrameworkCore;
#region EFCore.ModelBuilderExtensions Using
 
#endregion

namespace Further.Operation.Entityframework
{
    public static partial class ModelBuilderExtensions
    {
        public static void ConfigureOperation(this ModelBuilder builder)
        {
            #region EFCore.ModelBuilderExtensions PreOnModelCreating
             
            #endregion

            builder.ApplyConfiguration(new Further.Operation.Operations.EntityFrameworkCore.OperationConfiguration());
            builder.ApplyConfiguration(new Further.Operation.Operations.EntityFrameworkCore.OperationOwnerConfiguration());

            #region EFCore.ModelBuilderExtensions PostOnModelCreating
             
            #endregion
        }
    }
}

