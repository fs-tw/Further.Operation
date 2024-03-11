using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace Further.Operation
{
    public abstract partial class OperationRepository<TEntity, TKey> : EfCoreRepository<Further.Operation.EntityFrameworkCore.IOperationDbContext, TEntity, TKey>, IOperationRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        public OperationRepository(
            IDbContextProvider<Further.Operation.EntityFrameworkCore.IOperationDbContext> dbContextProvider
            ) : base(dbContextProvider)
        {
        }

        public virtual async Task<List<TEntity>> GetListAsync(
            ISpecification<TEntity> specification,
            bool includeDetails = false,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
                query = await WithDetailsAsync();

            query = specification.ApplyFilter(query);

            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? $"CreationTime desc" : sorting);

            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = await GetDbSetAsync();

            query = specification.ApplyFilter(query);

            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        #region Repositories.ProjectRepository Class
        
        #endregion
    }
}
