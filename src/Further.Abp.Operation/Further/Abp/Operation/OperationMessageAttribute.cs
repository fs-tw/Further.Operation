using FluentResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Abp.Operation
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class OperationMessageAttribute: OperationAttributeBase
    {
        public string? Message { get; set; }

        public override void UpdateOperationInfo(OperationInfo operationInfo, object methodResult)
        {
            if (Message != null)
            {
                var success = new Success(Message);

                foreach (var metadata in Metadata)
                {
                    success.WithMetadata(metadata.Key, metadata.Value);
                }

                operationInfo?.Result.WithSuccess(success);
            }
        }
    }
}
