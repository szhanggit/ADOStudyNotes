using System.Diagnostics.CodeAnalysis;

namespace Service.Extensions
{
    [ExcludeFromCodeCoverageAttribute]
    public static class GrpcRegistration
    {
        public static IServiceCollection AddGrpcRegistrations(this IServiceCollection services, IConfiguration configuration)
        {
            #region credit
            services.AddGrpcClient<TXC.Proto.Credit.CreditRpc.CreditRpcClient>(o =>
            {
                o.Address = new Uri(configuration["ServiceUrlConfiguration:ServiceCreditUrl"]);
            });
            #endregion
            return services;
        }
    }
}
