using FluentResults;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Json;

namespace Further.Abp.Operation
{
    public class OperationScope : IOperationScope, ITransientDependency
    {
        public const string OperationScopeReservationName = "_FurtherAbpOperationScope";

        public Guid Id { get; private set; }

        public IOperationScope? Outer { get; private set; }

        public bool IsReserved { get; set; }

        public bool IsDisposed { get; private set; }

        public bool IsCompleted { get; private set; }

        public string? ReservationName { get; set; }

        public OperationInfo? OperationInfo { get; set; }

        public event EventHandler<OperationScopeEventArgs> Disposed = default!;

        private Stopwatch stopwatch { get; set; }

        private readonly IServiceProvider serviceProvider;
        private readonly IGuidGenerator guidGenerator;
        private readonly IDistributedCache<OperationInfo, Guid> distributedCache;
        private readonly ILogger<OperationScope> logger;
        private readonly IJsonSerializer jsonSerializer;

        private OperationScopeOptions options { get; set; }

        public OperationScope(
            IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.guidGenerator = serviceProvider.GetRequiredService<IGuidGenerator>();
            this.distributedCache = serviceProvider.GetRequiredService<IDistributedCache<OperationInfo, Guid>>();
            this.logger = serviceProvider.GetRequiredService<ILogger<OperationScope>>();
            this.jsonSerializer = serviceProvider.GetRequiredService<IJsonSerializer>();
        }

        public virtual void Initialize(OperationScopeOptions? options = null, OperationInfoInitializeValue? value = null)
        {
            var id = value?.Id;

            if (Id == Guid.Empty && id == null)
            {
                Id = guidGenerator.Create();
                OperationInfo = new OperationInfo(Id);
            }

            if (Id != Guid.Empty && id != null)
            {
                throw new AbpException("不能對OperationScope重複初始化");
            }

            if (id != null)
            {
                Id = id.Value;
                OperationInfo = distributedCache.Get(Id);

                if (OperationInfo == null)
                {
                    OperationInfo = new OperationInfo(Id);
                }

                if (OperationInfo != null)
                {
                    distributedCache.Remove(Id);
                }
            }

            if (OperationInfo != null)
            {
                OperationInfo.OperationId = value?.OperationId;
                OperationInfo.OperationName = value?.OperationName;
            }

            this.options = options ?? new OperationScopeOptions();

            IsReserved = false;

            stopwatch = Stopwatch.StartNew();

            if (this.options.EnabledLogger)
            {
                logger.LogInformation("OperationScope {Id} Initialize.", Id);
            }
        }

        public virtual async Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                stopwatch.Stop();

                if (OperationInfo != null)
                {
                    OperationInfo.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                }

                if (options.SaveToCache && OperationInfo != null)
                {
                    await distributedCache.SetAsync(Id, OperationInfo, new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(options.MaxSurvivalTime)
                    });
                }

                if (!options.SaveToCache && OperationInfo != null)
                {
                    var operationStore = serviceProvider.GetRequiredService<IOperationStore>();

                    await operationStore.SaveAsync(OperationInfo, cancellationToken);
                }

                IsCompleted = true;

                if (options.EnabledLogger && OperationInfo != null)
                {
                    var operationInfoString = jsonSerializer.Serialize(OperationInfo);
                    logger.LogInformation("OperationScope {Id} completed.\n {Operation}",
                        Id, operationInfoString);
                }
            }
            catch (Exception ex)
            {
                if (options.EnabledLogger)
                {
                    logger.LogError(ex, "OperationScope {Id} complete error.", Id);
                }
                throw;
            }
        }

        public virtual void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            Disposed.Invoke(this, new OperationScopeEventArgs(this));

            if (this.options.EnabledLogger)
            {
                logger.LogInformation("OperationScope {Id} Dispose.", Id);
            }
        }

        public virtual void Reserve([NotNull] string reservationName)
        {
            Check.NotNullOrWhiteSpace(reservationName, nameof(reservationName));

            ReservationName = reservationName;
            IsReserved = true;
        }

        public virtual void SetOuter(IOperationScope? outer)
        {
            Outer = outer;
        }
    }
}
