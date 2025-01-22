using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Operation
{
    public static class OperationConsts
    {
        public const string PrrfixOperationIdKey = "OperationId";

        public const string PrrfixOperationValueKey = "OperationValue";
        public static string GetOperationIdKey(Guid operationId)
        {
            return $"{PrrfixOperationIdKey}_{operationId}";
        }

        public static string GetOperationValueKey(Guid operationId)
        {
            return $"{PrrfixOperationValueKey}_{operationId}";
        }
    }
}
