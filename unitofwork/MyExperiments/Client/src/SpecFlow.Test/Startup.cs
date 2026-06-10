using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow.Test
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
            //services.AddFluentValidation(Configuration);

            #region gRPC
            //services.AddGrpc();
            //services.AddGrpcReflection();
            #endregion gRPC            

            //services.AddgRPCService(Configuration);

            //services.AddHttpService();
            //services.AddDataOperation(Configuration);
            //services.AddServiceBus(Configuration);

            //services.AddFilter();
            //services.ConfigureTxcTenantConfigHelper();


            //services.AddSwagger(Configuration);
            //services.AddKeyVault(Configuration);
            //services.AddAzureStorage(Configuration);
            //services.ConfigureDirectoryConfig(Configuration);
            //services.AddApplicationInsight(Configuration);
            //services.RegisterFilter();
            //services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = true;
            //});
            //services.AddCached(Configuration);
            //services.AddSingleton<ITX2ServiceBusSender, TX2ServiceBusSender>();
            //services.AddSingleton<ITenantConfigHelper, TenantConfigHelper>();
        }
    }
}
