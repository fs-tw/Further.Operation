﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Further.Abp.Operation
{
    public interface IOperationProvider
    {
        Guid? CurrentId { get; }
        void Initialize(Guid? id = null);

        Task ModifyOperationAsync(Action<OperationInfo> action, TimeSpan? expiry = null, TimeSpan? wait = null, TimeSpan? retry = null);
        Task ModifyOperationAsync(Guid id, Action<OperationInfo> action, TimeSpan? expiry = null, TimeSpan? wait = null, TimeSpan? retry = null);

        Task<OperationInfo?> GetAsync(Guid id);

        Task<OperationInfo?> GetAsync(string id);

        Task RemoveAsync(Guid id);

        Task RemoveAsync(string id);
    }
}