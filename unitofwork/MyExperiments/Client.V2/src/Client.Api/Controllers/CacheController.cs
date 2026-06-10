using Client.Api.Constants;
using Client.Api.Controllers;
using Client.Api.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services.Command.Client;
using Services.Queries.Client;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ApiBaseController
    {
        private readonly IMemoryCache _localCache;
        public CacheController(IMemoryCache localCache, IMediator mediator) : base(mediator)
        {
            _localCache = localCache;
        }

        [HttpDelete]
        public IActionResult DeleteCache(CancellationToken cancellationToken)
        {
            if (_localCache is MemoryCache memoryCache)
            {
                var percentage = 1.0;//100%
                memoryCache.Compact(percentage);
            }
            return Ok("Finish");
        }
    }
}
