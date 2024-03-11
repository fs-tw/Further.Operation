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
    public partial class OperationOwnerConfiguration : IEntityTypeConfiguration<OperationOwner>
    {
        public void Configure(EntityTypeBuilder<OperationOwner> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(OperationDbProperties.DbTablePrefix + @"OperationOwners");
            builder.Property<Guid>(@"Id").HasColumnName(@"Id").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.OperationId).HasColumnName(@"OperationId").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.EntityType).HasColumnName(@"EntityType").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.EntityId).HasColumnName(@"EntityId").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.MetaData).HasColumnName(@"MetaData").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.TenantId).HasColumnName(@"TenantId").ValueGeneratedNever();
            builder.HasKey(@"Id");

            builder.ApplyObjectExtensionMappings();
        }
    }
}
