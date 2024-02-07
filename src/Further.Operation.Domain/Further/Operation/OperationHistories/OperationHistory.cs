using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Further.Operation.OperationHistories
{
    public class OperationHistory : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public string OperationName { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public Guid? TenantId { get; set; }
    }
}
