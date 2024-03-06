using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Uow;

namespace Further.Abp.Operation
{
    public static class OperationHelper
    {

        public static List<OperationInfoAttributeBase> GetOperationInfoAttributes(MethodInfo methodInfo)
        {
            return methodInfo
                .GetCustomAttributes(true)
                .OfType<OperationInfoAttributeBase>()
                .Where(x => !(x is OperationFailAttribute))
                .ToList();
        }

        public static OperationFailAttribute? GetOperationFailAttributes(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes(true).OfType<OperationFailAttribute>().FirstOrDefault();
        }

        public static bool IsOperationMethod(MethodInfo methodInfo)
        {
            if (HasOperationInfoAttribute(methodInfo))
            {
                return true;
            }

            return false;
        }

        public static bool IsOperationType(TypeInfo typeInfo)
        {
            if (AnyMethodHasOperationkAttribute(typeInfo))
            {
                return true;
            }

            return false;
        }

        public static bool ShouldIntercept(Type type)
        {
            if (DynamicProxyIgnoreTypes.Contains(type))
            {
                return false;
            }

            if (IsOperationType(type.GetTypeInfo()))
            {
                return true;
            }

            return false;
        }

        private static bool AnyMethodHasOperationkAttribute(TypeInfo implementationType)
        {
            return implementationType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(x => HasOperationInfoAttribute(x));
        }

        private static bool HasOperationInfoAttribute(MethodInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(OperationInfoAttributeBase), true);
        }
    }
}
