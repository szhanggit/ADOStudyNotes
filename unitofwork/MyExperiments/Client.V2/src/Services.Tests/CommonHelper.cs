using AutoMapper;
using Infrastructure.Mapper;
using Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Tests
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
