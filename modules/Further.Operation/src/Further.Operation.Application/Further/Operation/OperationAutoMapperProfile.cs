using System;
using Volo.Abp.Application.Dtos;
using AutoMapper;

namespace Further.Operation
{
    public partial class OperationAutoMapperProfile : Profile
    {
        public OperationAutoMapperProfile() 
        {

            CreateMap<Further.Operation.Operations.Operation, Further.Operation.Operations.OperationDto>();

            CreateMap<Further.Operation.Operations.OperationOwner, Further.Operation.Operations.OperationOwnerDto>();

            OnCreated();
        }

        partial void OnCreated();
    }

}
