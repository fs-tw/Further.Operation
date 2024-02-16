using Further.Operation.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace Further.Operation.Operations
{
    public class EfCoreOperationRepository : OperationRepository<Operation, Guid>, IOperationRepository
    {
        public EfCoreOperationRepository(
            IDbContextProvider<IOperationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<Operation>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }
    }
}
