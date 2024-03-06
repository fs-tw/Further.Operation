using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Abp.Operation
{
    public static class OperationKeyExtensions
    {
        public static string GetOperationBackUpKey(this string operationKey)
        {
            return $"{operationKey}:OperationBackUp";
        }
    }
}
