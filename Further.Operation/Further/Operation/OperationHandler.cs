using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;

namespace Further.Operation
{
    public class OperationHandler : IDistributedEventHandler<OperationExpiredEto>, ITransientDependency
    {
        public OperationHandler()
        {
        }
        public async Task HandleEventAsync(OperationExpiredEto eventData)
        {
        }
    }
}
