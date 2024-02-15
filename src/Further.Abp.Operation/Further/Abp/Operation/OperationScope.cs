using FluentResults;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Further.Abp.Operation
{
    public class OperationScope : IOperationScope, ITransientDependency
    {
        public Guid Id { get; private set; }

        public IOperationScope? Outer { get; private set; }

        public bool IsReserved { get; set; }

        public bool IsDisposed { get; private set; }

        public bool IsCompleted { get; private set; }

        public string? ReservationName { get; set; }

        public OperationInfo? OperationInfo { get; set; }

        public event EventHandler<OperationScopeEventArgs> Disposed = default!;

        private Exception? _exception;

        private readonly IServiceProvider serviceProvider;
        private readonly IGuidGenerator guidGenerator;
        private readonly IDistributedCache<OperationInfo, Guid> distributedCache;

        private OperationScopeOptions options { get; set; }

        public OperationScope(
            IServiceProvider serviceProvider,
            IGuidGenerator guidGenerator,
            IDistributedCache<OperationInfo, Guid> distributedCache)
        {
            this.serviceProvider = serviceProvider;
            this.guidGenerator = guidGenerator;
            this.distributedCache = distributedCache;
        }

        public void Initialize(OperationScopeOptions? options = null, OperationInfoInitializeValue? value = null)
        {
            var id = value?.Id;

            if (Id == Guid.Empty && id == null)
            {
                Id = guidGenerator.Create();
                OperationInfo = new OperationInfo(Id);
            }

            if (id != null)
            {
                Id = id.Value;
                OperationInfo = distributedCache.Get(Id);
            }

            if (Id != Guid.Empty && id != null)
            {
                throw new AbpException("不能對OperationScope重複初始化");
            }

            if (OperationInfo != null)
            {
                OperationInfo.OperationId = value?.OperationId;
                OperationInfo.OperationName = value?.OperationName;
            }

            this.options = options ?? new OperationScopeOptions();

            IsReserved = false;
        }

        public async Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (options.SaveToCache && OperationInfo != null)
                {
                    await distributedCache.SetAsync(Id, OperationInfo);
                }

                if (!options.SaveToCache && OperationInfo != null)
                {
                    var operationStore = serviceProvider.GetRequiredService<IOperationStore>();

                    await operationStore.SaveAsync(OperationInfo, cancellationToken);
                }

                IsCompleted = true;
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            Disposed.Invoke(this, new OperationScopeEventArgs(this));
        }

        public void Reserve([NotNull] string reservationName)
        {
            Check.NotNullOrWhiteSpace(reservationName, nameof(reservationName));

            ReservationName = reservationName;
            IsReserved = true;
        }

        public void SetOuter(IOperationScope? outer)
        {
            Outer = outer;
        }
    }
}
