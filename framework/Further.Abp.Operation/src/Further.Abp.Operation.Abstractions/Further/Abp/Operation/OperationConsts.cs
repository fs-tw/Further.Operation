using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Abp.Operation
{
    public static class OperationConsts
    {
        public static string GetIdKey(Guid operationId)
        {
            return $"Ids:{operationId}";
        }

        public static string GetValueKey(Guid operationId)
        {
            return $"Values:{operationId}";
        }
    }
}
