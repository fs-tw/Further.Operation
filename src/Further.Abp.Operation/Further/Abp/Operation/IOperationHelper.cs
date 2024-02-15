using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Further.Abp.Operation
{
    public interface IOperationHelper
    {
        bool IsOperationType(TypeInfo typeInfo);

        bool IsOperationMethod(MethodInfo methodInfo,out OperationAttributeBase? operationAttribute);
    }
}
