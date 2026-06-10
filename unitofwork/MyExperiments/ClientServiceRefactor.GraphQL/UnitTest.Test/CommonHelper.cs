using AutoMapper;
using Service.BusinessLogic;
using Service.Mapper;

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
