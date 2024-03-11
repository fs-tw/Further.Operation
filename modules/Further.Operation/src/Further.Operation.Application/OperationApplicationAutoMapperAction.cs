using AutoMapper;
using Further.Operation.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Json;
using Volo.Abp.ObjectMapping;

namespace Further.Operation
{
    public class SetOperationResult : IMappingAction<Further.Operation.Operations.Operation, OperationDto>
    {
        private readonly IJsonSerializer jsonSerializer;
        private readonly IObjectMapper objectMapper;

        public SetOperationResult(
            IJsonSerializer jsonSerializer,
            IObjectMapper objectMapper)
        {
            this.jsonSerializer = jsonSerializer;
            this.objectMapper = objectMapper;
        }

        public void Process(Operations.Operation source, OperationDto destination, ResolutionContext context)
        {
            var operationResult = source.GetResult(jsonSerializer);

            destination.OperationResult = objectMapper.Map<OperationResult, OperationResultDto>(operationResult);
        }
    }
}
