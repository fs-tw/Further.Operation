using AutoFilterer.Enums;
using Further.Abp.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using System.Linq.Dynamic.Core;
using System.Collections;

namespace Further.Operation.Operations
{
    public class OperationStore : ITransientDependency
    {
        private readonly IOperationProvider operationProvider;
        private readonly IOperationRepository operationRepository;
        private readonly OperationManager operationManager;

        public OperationStore(
            IOperationProvider operationProvider,
            IOperationRepository operationRepository,
            OperationManager operationManager)
        {
            this.operationProvider = operationProvider;
            this.operationRepository = operationRepository;
            this.operationManager = operationManager;
        }

        public async Task<Operation> GetAsync(Guid id)
        {
            var operationInfo = await operationProvider.GetAsync(id);

            if (operationInfo != null)
            {
                return await operationManager.CreateAsync(operationInfo);
            }

            return await operationRepository.GetAsync(id);
        }

        public async Task<List<Operation>> GetListAsync(OperationFilterBase<Operation>? filter = null, int maxResultCount = int.MaxValue, int skipCount = 0, string? sorting = null)
        {
            var operations = await GetRedisOperationAsync(filter);
            var redisCount = operations.Count;

            if (redisCount >= maxResultCount)
            {
                var operationQuery = operations.AsQueryable();
                operationQuery = ApplySorting(operationQuery, sorting);
                return operationQuery.PageBy(skipCount, maxResultCount).ToList();
            }

            var adjustedSkipCount = Math.Max(0, skipCount - redisCount);
            var adjustedMaxResultCount = Math.Max(1, maxResultCount - redisCount);

            var operationDatas = await operationRepository.GetListAsync(
                specification: filter,
                includeDetails: true,
                maxResultCount: adjustedMaxResultCount,
                skipCount: adjustedSkipCount,
                sorting: sorting);

            operations.AddRange(operationDatas);

            var operationQueryFinal = operations.AsQueryable();
            operationQueryFinal = ApplySorting(operationQueryFinal, sorting);

            var finalSkipCount = Math.Min(skipCount, redisCount);

            return operationQueryFinal.PageBy(finalSkipCount, maxResultCount).ToList();
        }

        public async Task<long> GetCountAsync(OperationFilterBase<Operation>? filter = null)
        {

            var operations = await GetRedisOperationAsync(filter);

            var operationDataCount = await operationRepository.GetCountAsync(filter);

            operationDataCount += operations.Count;

            return operationDataCount;
        }

        protected virtual async Task<List<Operation>> GetRedisOperationAsync(OperationFilterBase<Operation>? filter = null)
        {
            var operationIds = await operationProvider.ListIdsAsync();

            var operations = new List<Operation>();

            foreach (var operationId in operationIds)
            {
                var operation = await GetAsync(operationId);

                if (operation != null)
                {
                    operations.Add(operation);
                }
            }

            if (filter != null)
            {
                operations = filter.ApplyFilter(operations.AsQueryable()).ToList();
            }

            return operations;
        }

        protected IQueryable<TSource> ApplySorting<TSource>(IQueryable<TSource> query, string? sorting = null, string? defaultSorting = null)
        {
            var sortingUsed = defaultSorting ?? "CreationTime desc";

            if (IsValidSortingParameter(sorting, typeof(TSource)))
            {
                query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? sortingUsed : sorting);
            }
            else
            {
                query = query.OrderBy(sortingUsed);
            }

            return query;
        }

        protected virtual bool IsValidSortingParameter(string? sorting, Type entityType)
        {
            if (string.IsNullOrWhiteSpace(sorting))
            {
                return true;
            }

            var properties = GetProperties(entityType);
            var sortingFields = sorting.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(field => field.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]);

            return sortingFields.All(field => properties.Contains(field, StringComparer.OrdinalIgnoreCase));
        }

        protected virtual List<string> GetProperties(Type type, string parentProperty = "")
        {
            var properties = new List<string>();
            foreach (var prop in type.GetProperties())
            {
                var currentProperty = string.IsNullOrEmpty(parentProperty) ? prop.Name : $"{parentProperty}.{prop.Name}";
                properties.Add(currentProperty);

                if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                {
                    properties.AddRange(GetProperties(prop.PropertyType, currentProperty));
                }
            }
            return properties;
        }
    }
}
