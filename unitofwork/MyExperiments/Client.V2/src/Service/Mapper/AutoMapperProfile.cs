using AutoMapper;
using Domain.DTOs;

namespace Service.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //CreateMap<Source,Destination>()
            CreateMap<TXC.Proto.Client.GetClientListResponse, Domain.Models.Response.GetClientListResponse>()
                .ForMember(s => s.TotalCount, c => c.MapFrom(m => m.TotalCount))
                .ForMember(s => s.ClientDtos, c => c.MapFrom(m => m.ClientDtos));
            CreateMap<TXC.Proto.Client.ClientListItem, ClientDto>();

            CreateMap<TXC.Proto.Client.CreateClientRequest, TXC.Common.MessageContract.Client.ClientMessageV1>();
            CreateMap<TXC.Proto.Client.UpdateClientRequest, TXC.Common.MessageContract.Client.ClientMessageV1>();
        }
    }
}
