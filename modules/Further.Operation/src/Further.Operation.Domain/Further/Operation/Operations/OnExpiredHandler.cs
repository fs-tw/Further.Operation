using Further.Abp.Operation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace Further.Operation.Operations
{
    public class OnExpiredHandler : IDistributedEventHandler<OperationExpiredEvent>, ITransientDependency
    {
        private readonly IOperationRepository operationRepository;
        private readonly OperationManager operationManager;
        private readonly IUnitOfWorkManager unitOfWorkManager;

        public OnExpiredHandler(
            IOperationRepository operationRepository,
            OperationManager operationManager,
            IUnitOfWorkManager unitOfWorkManager)
        {
            this.operationRepository = operationRepository;
            this.operationManager = operationManager;
            this.unitOfWorkManager = unitOfWorkManager;
        }

        public async Task HandleEventAsync(OperationExpiredEvent eventData)
        {
            var operation = await operationManager.CreateAsync(eventData.OperationInfo);

            using (var uow = unitOfWorkManager.Begin(requiresNew: true))
            {
                await operationRepository.InsertAsync(operation);
                await uow.CompleteAsync();
            }
        }
    }
}
