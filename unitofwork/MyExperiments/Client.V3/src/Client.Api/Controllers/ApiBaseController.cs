using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Client.Api.Filters;
using System.Linq;
using System.Security.Claims;

namespace Client.Api.Controllers
{
    //[Authorize]
    [ServiceFilter(typeof(ModelValidationResultFilter))]    
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiBaseController : ControllerBase
    {
        protected readonly IMediator mediator;
        public ApiBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        internal string FlattenModelStateErrorMessage()
        {
            return string.Join(" | ", ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage));
        }

        internal int GetUserIdClaim()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userId = identity.FindFirst(ClaimTypes.Sid)?.Value;

                if(!string.IsNullOrEmpty(userId))
                    return int.Parse(userId);
            }

            return 0;
        }
    }
}
