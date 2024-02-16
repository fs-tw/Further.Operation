using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Further.Operation.Operations
{
    public static partial class QueryableOfOperationExtenstions
    {
        public static IQueryable<Operation> IncludeDetails(this IQueryable<Operation> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            IQueryable<Operation> result = queryable
                .Include(x => x.OperationOwners);

            return result;
        }
    }
}
