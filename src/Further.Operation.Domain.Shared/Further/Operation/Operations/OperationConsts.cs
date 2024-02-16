using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
namespace Further.Operation.Operations
{
    public static partial class OperationConsts
    {
        private const string DefaultSorting = "{0}CreationTime desc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Operation." : string.Empty);
        }

        #region EFCore.EntityConsts
        public const int OperationIdMaxLength = int.MaxValue;
        public const int OperationNameMaxLength = int.MaxValue;
        public const int ResultMaxLength = int.MaxValue;
        #endregion
    }
}
