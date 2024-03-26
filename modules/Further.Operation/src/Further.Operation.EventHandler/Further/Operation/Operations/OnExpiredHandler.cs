using Further.Abp.Operation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace Further.Operation.Operations
{
    public class OnExpiredHandler : IDistributedEventHandler<OperationExpiredEto>, ITransientDependency
    {
        private readonly IOperationRepository operationRepository;
        private readonly OperationManager operationManager;
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly ILogger<OnExpiredHandler> logger;

        public OnExpiredHandler(
            IOperationRepository operationRepository,
            OperationManager operationManager,
            IUnitOfWorkManager unitOfWorkManager,
            ILogger<OnExpiredHandler> logger)
        {
            this.operationRepository = operationRepository;
            this.operationManager = operationManager;
            this.unitOfWorkManager = unitOfWorkManager;
            this.logger = logger;
        }

        public virtual async Task HandleEventAsync(OperationExpiredEto eventData)
        {
            try
            {
                logger.LogInformation($"Operation存檔開始 {eventData.OperationInfo.OperationName}({eventData.OperationInfo.Id})");

                var operation = await operationManager.CreateAsync(eventData.OperationInfo);

                using (var uow = unitOfWorkManager.Begin(requiresNew: true))
                {
                    await operationRepository.InsertAsync(operation);
                    await uow.CompleteAsync();
                }

                logger.LogInformation($"Operation存檔完成 {eventData.OperationInfo.OperationName}({eventData.OperationInfo.Id}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Operation存檔失敗 {eventData.OperationInfo.OperationName}({eventData.OperationInfo.Id}");
            }
        }
    }
}
