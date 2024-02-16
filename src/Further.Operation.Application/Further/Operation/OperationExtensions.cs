using System.Collections.Generic;
using System;
using System.Linq;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectMapping;

namespace Further.Operation
{
    public static partial class OperationExtensions
    {
        public static Further.Operation.Operations.OperationDto ToDto(this Further.Operation.Operations.Operation source, IObjectMapper objectMapper)
        {
            return objectMapper.Map<Further.Operation.Operations.Operation, Further.Operation.Operations.OperationDto>(source);
        }

        public static List<Further.Operation.Operations.OperationDto> ToDtos(this IEnumerable<Further.Operation.Operations.Operation> source, IObjectMapper objectMapper)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToDto(objectMapper))
              .ToList();

            return target;
        }
        public static Further.Operation.Operations.OperationOwnerDto ToDto(this Further.Operation.Operations.OperationOwner source, IObjectMapper objectMapper)
        {
            return objectMapper.Map<Further.Operation.Operations.OperationOwner, Further.Operation.Operations.OperationOwnerDto>(source);
        }

        public static List<Further.Operation.Operations.OperationOwnerDto> ToDtos(this IEnumerable<Further.Operation.Operations.OperationOwner> source, IObjectMapper objectMapper)
        {
            if (source == null)
              return null;

            var target = source
              .Select(src => src.ToDto(objectMapper))
              .ToList();

            return target;
        }
    }
}
