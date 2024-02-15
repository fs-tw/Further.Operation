using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Further.Abp.Operation
{
    public class OperationScopeProvider : IOperationScopeProvider, ISingletonDependency
    {
        private readonly IAmbientOperationScope ambientOperationInfo;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public IOperationScope? CurrentScope => ambientOperationInfo.GetCurrentOperationScope();

        public OperationInfo? Current => ambientOperationInfo.GetOperationInfo();

        public OperationScopeProvider(
            IAmbientOperationScope ambientOperationInfo,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.ambientOperationInfo = ambientOperationInfo;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public IOperationScope Begin(bool requiresNew = false)
        {
            var currentOperationInfo = CurrentScope;

            if (currentOperationInfo != null && !requiresNew)
            {
                return new ChildOperationScope(currentOperationInfo);
            }

            return CreateNewOperationScope();
        }

        public IOperationScope Reserve(string reservationName, bool requiresNew = false)
        {
            Check.NotNull(reservationName, nameof(reservationName));

            if (!requiresNew &&
                ambientOperationInfo.OperationScope != null &&
                ambientOperationInfo.OperationScope.IsReservedFor(reservationName))
            {
                return new ChildOperationScope(ambientOperationInfo.OperationScope);
            }

            var operationInfo = CreateNewOperationScope();
            operationInfo.Reserve(reservationName);

            return operationInfo;
        }

        public void BeginReserved(string reservationName)
        {
            if (!TryBeginReserved(reservationName))
            {
                throw new AbpException($"Could not begin reserved operation for '{reservationName}'");
            }
        }

        public bool TryBeginReserved(string reservationName)
        {
            Check.NotNull(reservationName, nameof(reservationName));

            var operationInfo = ambientOperationInfo.OperationScope;

            while (operationInfo != null && !operationInfo.IsReservedFor(reservationName))
            {
                operationInfo = operationInfo.Outer;
            }

            if (operationInfo == null)
            {
                return false;
            }

            operationInfo.Initialize();

            return true;
        }

        private IOperationScope CreateNewOperationScope()
        {
            var scope = serviceScopeFactory.CreateScope();
            try
            {
                var outerOperationInfo = ambientOperationInfo.OperationScope;

                var operationInfo = scope.ServiceProvider.GetRequiredService<IOperationScope>();

                operationInfo.SetOuter(outerOperationInfo);

                ambientOperationInfo.SetOperationScope(operationInfo);

                operationInfo.Disposed += (sender, args) =>
                {
                    ambientOperationInfo.SetOperationScope(outerOperationInfo);
                    scope.Dispose();
                };

                return operationInfo;
            }
            catch
            {
                scope.Dispose();
                throw;
            }
        }
    }
}
