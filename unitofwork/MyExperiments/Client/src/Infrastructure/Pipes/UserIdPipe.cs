using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Services;

namespace Infrastructure.Pipes
{
    public class UserIdPipe<TIn, TOut> : IPipelineBehavior<TIn, TOut>
    {

        private HttpContext httpContext;
        public UserIdPipe(IHttpContextAccessor accessor)
        {
            httpContext = accessor.HttpContext;
        }
        public async Task<TOut> Handle(TIn request, CancellationToken cancellationToken, RequestHandlerDelegate<TOut> next)
        {
            //var userId = httpContext.User.Claims.FirstOrDefault(f => f.Type.Equals(ClaimTypes.NameIdentifier)).Value;

            if (request is BaseRequest br)
            {
                // do some validation for input
                //br.UserId = "pupel";
            }

            var result = await next();

            //if (result is Response<SampleInfoModel> userInfoResponse)
            //{
            //    userInfoResponse.Data.UserName += " CHECKED";
            //}
            return result;
        }
    }
}
