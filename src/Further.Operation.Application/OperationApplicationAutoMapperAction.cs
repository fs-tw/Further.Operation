using AutoMapper;
using Further.Operation.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Json;

namespace Further.Operation
{
    public class SetOperationResult : IMappingAction<Further.Operation.Operations.Operation, OperationDto>
    {
        private readonly IJsonSerializer jsonSerializer;

        public SetOperationResult(
            IJsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
        }

        public void Process(Operations.Operation source, OperationDto destination, ResolutionContext context)
        {
            destination.OperationResult = source.GetResult(jsonSerializer);
        }
    }
}
