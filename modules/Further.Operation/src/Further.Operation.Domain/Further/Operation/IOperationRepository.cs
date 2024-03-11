using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace Further.Operation
{
    public static partial class OperationRepositoryExtensions
    {
        public static IQueryable<T> ApplyFilter<T>(this ISpecification<T> specification, IQueryable<T> query)
        {
            if (specification != null)
            {
                query = query.Where(specification.ToExpression());
            }

            return query;
        }
    }

    public partial interface IOperationRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        Task<List<TEntity>> GetListAsync(
            ISpecification<TEntity> specification,
            bool includeDetails = false,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            ISpecification<TEntity> specification,
            CancellationToken cancellationToken = default);

        #region Repositories.ProjectRepository Interface
        
        #endregion
    }
}
