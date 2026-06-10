using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using TXC.Common.Domain;
using TXC.Common.Services;

namespace Client.Api.Filters
{
    public class TenantFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var TenantBasicInfoId = context.HttpContext?.Request?.Headers[HeaderConstants.TenantId];
            var TenantName = context.HttpContext?.Request?.Headers[HeaderConstants.TenantName];

            if (string.IsNullOrEmpty(TenantBasicInfoId) || string.IsNullOrEmpty(TenantName)
                || string.IsNullOrWhiteSpace(TenantBasicInfoId) || string.IsNullOrWhiteSpace(TenantName))
            {
                var response = Response.Fail<int>("Tenant Header Not Found", (int)HttpStatusCode.BadRequest);
                context.Result = new BadRequestObjectResult(response);
                return;
            }
        }
    }
}
