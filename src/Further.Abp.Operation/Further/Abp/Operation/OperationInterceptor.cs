using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Further.Abp.Operation
{
    public class OperationInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public OperationInterceptor(
            IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public override Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            throw new NotImplementedException();
        }
    }
}
