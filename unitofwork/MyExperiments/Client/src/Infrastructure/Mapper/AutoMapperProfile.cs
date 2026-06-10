using AutoMapper;
using Domain.Dto;
using TXC.Common.MessageContract.Client;
using txc_common_lib.Utilities;

namespace Infrastructure.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //CreateMap<Source,Destination>()
            //CreateMap<Services.Command.Client.CreateClientCommand, ClientMessageV1>();
            //CreateMap<Services.Command.Client.UpdateClientCommand, ClientMessageV1>();

            //CreateMap<Services.Command.Client.CreateClientCommand, TXC.Proto.Client.CreateClientRequest>();
            //CreateMap<Services.Command.Client.UpdateClientCommand, TXC.Proto.Client.UpdateClientRequest>();
            //CreateMap<Services.Queries.Client.GetClientListQuery, TXC.Proto.Client.GetClientListRequest>();
            CreateMap<TXC.Proto.Client.GetClientListResponse, Domain.Models.Response.GetClientListResponse>()
                .ForMember(s => s.TotalCount, c => c.MapFrom(m => m.TotalCount))
                .ForMember(s => s.ClientDtos, c => c.MapFrom(m => m.ClientDtos));
            CreateMap<TXC.Proto.Client.ClientListItem, ClientDto>();

            CreateMap<TXC.Proto.Client.CreateClientRequest, TXC.Common.MessageContract.Client.ClientMessageV1>();
            CreateMap<TXC.Proto.Client.UpdateClientRequest, TXC.Common.MessageContract.Client.ClientMessageV1>();
        }
    }
}
