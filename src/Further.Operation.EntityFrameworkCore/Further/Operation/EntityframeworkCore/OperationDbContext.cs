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
using Volo.Abp.DependencyInjection;

namespace Further.Operation.EntityFrameworkCore
{

    [ConnectionStringName(OperationDbProperties.ConnectionStringName)]
    #region EFCore.DbContext Declare
    public partial class OperationDbContext : Volo.Abp.EntityFrameworkCore.AbpDbContext<OperationDbContext>, IOperationDbContext 
    #endregion
    {

        public OperationDbContext(DbContextOptions<OperationDbContext> options) :
            base(options)
        {
        }
        public virtual DbSet<Further.Operation.Operations.Operation> Operations
        {
            get;
            set;
        }
        public virtual DbSet<Further.Operation.Operations.OperationOwner> OperationOwners
        {
            get;
            set;
        }
        #region EFCore.DbContext ObjectServices
        
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ConfigureOperation();
        }

        public bool HasChanges()
        {
            return ChangeTracker.Entries().Any(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added || e.State == Microsoft.EntityFrameworkCore.EntityState.Modified || e.State == Microsoft.EntityFrameworkCore.EntityState.Deleted);
        }
    }
}
