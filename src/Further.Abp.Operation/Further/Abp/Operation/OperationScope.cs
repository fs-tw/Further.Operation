using FluentResults;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Further.Abp.Operation
{
    public class OperationScope : IOperationScope, ITransientDependency
    {
        public Guid Id { get; } = Guid.NewGuid();

        public IOperationScope? Outer { get; private set; }

        public bool IsReserved { get; set; }

        public bool IsDisposed { get; private set; }

        public bool IsCompleted { get; private set; }

        public string? ReservationName { get; set; }

        public OperationInfo? OperationInfo { get; set; }

        public event EventHandler<OperationInfoEventArgs> Disposed = default!;

        private Exception? _exception;

        private readonly IServiceProvider serviceProvider;

        public OperationScope(
            IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Initialize()
        {
            IsReserved = false;
        }

        public async Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var operationStore = serviceProvider.GetRequiredService<IOperationStore>();

                await operationStore.SaveAsync(OperationInfo, cancellationToken);

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

            if (!IsCompleted && _exception == null)
            {
                throw new AbpException("請結束本次操作");
            }

            Disposed.Invoke(this, new OperationInfoEventArgs(this));
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
