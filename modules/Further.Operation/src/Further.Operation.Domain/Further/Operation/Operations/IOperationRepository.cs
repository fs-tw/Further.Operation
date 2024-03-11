using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace Further.Operation.Operations
{
    public partial interface IOperationRepository : IOperationRepository<Further.Operation.Operations.Operation, Guid>
    {

       #region Repositories.EntityRepository Interface
        
        #endregion
    }
}
