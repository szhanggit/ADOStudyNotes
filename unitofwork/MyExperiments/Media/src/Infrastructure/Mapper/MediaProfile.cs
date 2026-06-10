using AutoMapper;
using Domain.Dto;
using Domain.Models.Request;
using Domain.Models.Response;
using Services.Command.ImageMedia;
using Services.Queries.TxMedia;
using TXC.Proto.Media;

namespace Infrastructure.Mapper
{
    public class MediaProfile : Profile
    {
        public MediaProfile()
        {
            CreateMap<RenameMediaRequest, RenameImageMediaCommand>().ReverseMap();
            CreateMap<TxMediaDto, GetMediaByIdResponse>().ReverseMap();
            CreateMap<GetMediaByIdRequest, GetTxMediaQuery>().ReverseMap();
            CreateMap<GetTxMediaListQuery, GetAllMediaRequest>()
                .ForMember(d => d.MediaCategory, opt => opt.MapFrom(s => (int)s.MediaCategory))
                .ForMember(d => d.SearchKey , opt => opt.MapFrom(s => s.SearchKey));
            CreateMap<GetAllMediaItem, TxMediaDto>().ReverseMap();


            CreateMap<GetAllMediaRequest, GetAllMediaRequestModel>().ReverseMap();
            CreateMap<GetAllMediaItem, GetMediaResponseModel>().ReverseMap();
            CreateMap<GetMediaByIdResponse, GetMediaResponseModel>().ReverseMap();
            
        }
    }
}
