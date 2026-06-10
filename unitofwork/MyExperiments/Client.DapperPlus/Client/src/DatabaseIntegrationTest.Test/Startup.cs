using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RespositoryTest.Test
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            //services.ConfigureDataOperations(Configuration);
            //services.ConfigurePipe();
            //services.ConfigureMediateR();
            //services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddControllers();
            
        }
    }
}
