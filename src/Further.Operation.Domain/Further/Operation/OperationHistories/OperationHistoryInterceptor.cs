using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Further.Operation.OperationHistories
{
    public class OperationHistoryInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public OperationHistoryInterceptor(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var targetObject = invocation.TargetObject;

                var methodInfo = invocation.Method;

                var attribute = methodInfo.GetCustomAttributes(true).OfType<OperationHistoryAttribute>().FirstOrDefault();

                var entityType = attribute?.EntityType;

                await invocation.ProceedAsync();

                var result = invocation.ReturnValue;
            }
        }
    }
}
