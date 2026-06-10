using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMedia.Api.Constants;
using Services.Command.ImageMedia;
using Services.Queries.ImageMedia;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Logging;
using txc_common_lib.Filters.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ApiBaseController
    {

        private readonly ITelemetryLogTrace<MediaController> telemetryTrace;
        private readonly TelemetryClient _telemetryClient;
        public MediaController(IMediator mediator
            , ITelemetryLogTrace<MediaController> telemetryTrace
            , TelemetryClient telemetryClient) : base(mediator)
        {
            this.telemetryTrace = telemetryTrace;
            _telemetryClient = telemetryClient;

        }

        [Authorize(AuthenticationSchemes = AuthenticationConstants.AllAuthScheme)]
        [ServiceFilter(typeof(TenantFilter))]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetImageMediaListQuery query, CancellationToken cancellationToken)
        {
            using (var operation = _telemetryClient.StartOperation<RequestTelemetry>("GetImageMediaList"))
            {
                telemetryTrace.LogTrace("Get media image list started", "Query", query, SeverityLevel.Information);
                var result = await mediator.Send(query, cancellationToken);
                telemetryTrace.LogTrace("Get media image list finished", "ResponseBody", result, SeverityLevel.Information);
                return Ok(result);
            }
        }

        [Authorize(AuthenticationSchemes = AuthenticationConstants.AllAuthScheme)]
        [ServiceFilter(typeof(TenantFilter))]
        [HttpPut]
        public async Task<IActionResult> Put(RenameImageMediaCommand command, CancellationToken cancellationToken)
        {
            using (var operation = _telemetryClient.StartOperation<RequestTelemetry>("RenameImageMedia"))
            {
                telemetryTrace.LogTrace("Rename image started", "RequestBody", command, SeverityLevel.Information);
                var result = await mediator.Send(command, cancellationToken);

                if (result.Success)
                {
                    telemetryTrace.LogTrace("Rename image finished successfully", "ResponseBody", result, SeverityLevel.Information);
                    return Ok(result);
                }
                else
                {
                    telemetryTrace.LogTrace("Rename image finished with errors", "ResponseBody", result, SeverityLevel.Information);
                    return BadRequest(result);
                }
            }
        }

        [Authorize(AuthenticationSchemes = AuthenticationConstants.AllAuthScheme)]
        [ServiceFilter(typeof(TenantFilter))]
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> Get([FromQuery] GetMediaQuery query, CancellationToken cancellationToken)
        {
            using (var operation = _telemetryClient.StartOperation<RequestTelemetry>("GetMedia"))
            {
                telemetryTrace.LogTrace("Get media image started", "Query", query, SeverityLevel.Information);
                var result = await mediator.Send(query, cancellationToken);
                telemetryTrace.LogTrace("Get media image finished", "ResponseBody", result, SeverityLevel.Information);
                return Ok(result);
            }
        }
    }
}
