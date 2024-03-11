using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Further.Operation.Operations.EntityFrameworkCore
{
    public partial class OperationConfiguration : IEntityTypeConfiguration<Operation>
    {
        public void Configure(EntityTypeBuilder<Operation> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(OperationDbProperties.DbTablePrefix + @"Operations");
            builder.Property<Guid>(@"Id").HasColumnName(@"Id").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.OperationId).HasColumnName(@"OperationId").ValueGeneratedNever();
            builder.Property(x => x.OperationName).HasColumnName(@"OperationName").ValueGeneratedNever();
            builder.Property(x => x.Result).HasColumnName(@"Result").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.IsSuccess).HasColumnName(@"IsSuccess").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.ExecutionDuration).HasColumnName(@"ExecutionDuration").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.TenantId).HasColumnName(@"TenantId").ValueGeneratedNever();
            builder.HasKey(@"Id");
            builder.HasMany(x => x.OperationOwners).WithOne().HasForeignKey(@"OperationId").IsRequired(true);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
