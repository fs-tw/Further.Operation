using AutoMapper;
using Further.Operation.Operations;
using Volo.Abp.AutoMapper;

namespace Further.Operation;

public class OperationApplicationAutoMapperProfile : Profile
{
    public OperationApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<OperationReason,OperationReasonDto>();

        CreateMap<OperationResult,OperationResultDto>();


        CreateMap<Further.Operation.Operations.Operation, Further.Operation.Operations.OperationDto>()
            .Ignore(x => x.OperationResult)
            .AfterMap<SetOperationResult>();
    }
}
