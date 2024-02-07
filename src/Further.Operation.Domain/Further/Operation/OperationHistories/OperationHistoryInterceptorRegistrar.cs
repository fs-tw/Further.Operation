using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Further.Operation.OperationHistories
{
    public static class OperationHistoryInterceptorRegistrar
    {
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            if (ShouldIntercept(context.ImplementationType))
            {
                context.Interceptors.TryAdd<OperationHistoryInterceptor>();
            }
        }

        private static bool ShouldIntercept(Type type)
        {
            if (DynamicProxyIgnoreTypes.Contains(type))
            {
                return false;
            }

            if (type.IsDefined(typeof(OperationHistoryAttribute), true))
            {
                return true;
            }

            if (type.GetMethods().Any(m => m.IsDefined(typeof(OperationHistoryAttribute), true)))
            {
                return true;
            }

            return false;
        }
    }
}
