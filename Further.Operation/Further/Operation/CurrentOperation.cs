using FluentResults;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Further.Operation
{
    public class CurrentOperation : ICurrentOperation, ITransientDependency
    {
        private readonly OperationOptions options;
        private readonly ICurrentOperationAccessor _currentOperationAccessor;
        private readonly IDistributedCache<BasicOperationInfo> distributedCache;
        private readonly ILogger<CurrentOperation> _logger;

        public CurrentOperation(
            IOptions<OperationOptions> options,
            ICurrentOperationAccessor currentOperationAccessor,
            IDistributedCache<BasicOperationInfo> distributedCache,
            ILogger<CurrentOperation> logger
            )
        {
            this.options = options.Value;
            this._currentOperationAccessor = currentOperationAccessor;
            this.distributedCache = distributedCache;
            this._logger = logger;
        }

        public Guid? Id => _currentOperationAccessor.Current?.Id;

        public string? Name => _currentOperationAccessor.Current?.Name;

        public IResultBase? Result => _currentOperationAccessor.Current?.Result;

        public IReadOnlyCollection<OperationCorrelationInfo>? Correlations => _currentOperationAccessor.Current?.Correlations;

        public async Task SaveAsync(Action<BasicOperationInfo> action)
        {
            if (this.Id.HasValue)
            {
                try
                {
                    await distributedCache.GetOrAddAsync(
                        OperationConsts.GetIdKey(this.Id.Value),
                        async () =>
                        {
                            await distributedCache.SetAsync(OperationConsts.GetValueKey(this.Id.Value), new BasicOperationInfo(this.Id.Value), new DistributedCacheEntryOptions
                            {
                                SlidingExpiration = options.DefaultSlidingExpiration + TimeSpan.FromSeconds(10)
                            });

                            return new BasicOperationInfo(new Guid());
                        },
                        () => new DistributedCacheEntryOptions { SlidingExpiration = options.DefaultSlidingExpiration });

                    this._currentOperationAccessor.Current = await distributedCache.GetAsync(OperationConsts.GetValueKey(this.Id.Value));

                    var item = await distributedCache.GetAsync(OperationConsts.GetValueKey(this.Id.Value));


                    var current = _currentOperationAccessor.Current!;

                    action?.Invoke(current);

                    await distributedCache.SetAsync(OperationConsts.GetValueKey(this.Id.Value), current, new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = options.DefaultSlidingExpiration + TimeSpan.FromSeconds(10)
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while modifying the operation info.");
                    throw;
                }
            }
        }


    }
}
