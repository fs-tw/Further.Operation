using FluentResults;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace Further.Operation
{
    public interface ICurrentOperation
    {
        Guid? Id { get; }

        string? Name { get; }

        IResultBase? Result { get; }

        IReadOnlyCollection<OperationCorrelationInfo>? Correlations { get; }

        Task SaveAsync(Action<BasicOperationInfo>? action);
    }
}
