using AutoMapper;
using Infrastructure.Mapper;
using Services.Core;

namespace UnitTest.Test
{
    public class CommonHelper
    {
        protected IObjectConvertingService GetObjectConvertingService()
        {
            MapperConfiguration mapperConfig = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            IMapper _mapper = new Mapper(mapperConfig);
            IObjectConvertingService ObjectConvertingService = new ObjectConvertingService(_mapper);
            return ObjectConvertingService;
        }
    }
}
