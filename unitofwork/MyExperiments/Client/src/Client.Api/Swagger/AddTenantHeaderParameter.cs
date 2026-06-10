using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ServiceClient.Api.Config;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using TXC.Common.Domain;

namespace ServiceClient.Api.Swagger
{
    public class AddTenantHeaderParameter : IOperationFilter
    {
        private readonly TxcSwaggerConfiguration _swaggerConfiguration;
        public AddTenantHeaderParameter(IOptions<TxcSwaggerConfiguration> configuration)
        {
            _swaggerConfiguration = configuration.Value;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            if (_swaggerConfiguration.ExcludeTenantHeaders != null && !_swaggerConfiguration.ExcludeTenantHeaders.Contains(operation.Tags[0].Name))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = HeaderConstants.TenantId,
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema() { Type = "string" },
                    Required = true
                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = HeaderConstants.TenantName,
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema() { Type = "string" },
                    Required = true
                });
            }
        }
    }
}
