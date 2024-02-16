using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Further.Operation.Entityframework
{
    [ConnectionStringName(OperationDbProperties.ConnectionStringName)]
    #region EFCore.IDbContext Declare
    public partial interface IOperationDbContext : IEfCoreDbContext 
    #endregion
    {

        DbSet<Further.Operation.Operations.Operation> Operations
        {
            get;
        }

        DbSet<Further.Operation.Operations.OperationOwner> OperationOwners
        {
            get;
        }

        #region EFCore.IDbContext ObjectServices
        
        #endregion
    }
}
