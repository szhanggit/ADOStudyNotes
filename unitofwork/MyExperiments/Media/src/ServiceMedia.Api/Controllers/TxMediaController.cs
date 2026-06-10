using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMedia.Api.Constants;
using Services.Command.ImageMedia;
using Services.Queries.TxMedia;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Logging;
using txc_common_lib.Filters.Models;

namespace ServiceMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TxMediaController : ApiBaseController
    {
        private readonly ITelemetryLogTrace<TxMediaController> telemetryTrace;
        private readonly TelemetryClient _telemetryClient;
        public TxMediaController(IMediator mediator
            , ITelemetryLogTrace<TxMediaController> telemetryTrace
            , TelemetryClient telemetryClient) : base(mediator)
        {
            this.telemetryTrace = telemetryTrace;
            _telemetryClient = telemetryClient;
        }

        [ServiceFilter(typeof(TenantFilter))]
        //[Authorize(AuthenticationSchemes = AuthenticationConstants.Connector)]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetTxMediaListQuery query, CancellationToken cancellationToken)
        {
            using (var operation = _telemetryClient.StartOperation<RequestTelemetry>("GetListMediaQuerry-Connector"))
            {
                telemetryTrace.LogTrace("Get media image list for TX2 started", "Query", query, SeverityLevel.Information);
                var result = await mediator.Send(query, cancellationToken);
                telemetryTrace.LogTrace("Get media image list for TX2 list finished", "ResponseBody", result, SeverityLevel.Information);
                return Ok(result);
            }
           
        }

        [ServiceFilter(typeof(TenantFilter))]
        //[Authorize(AuthenticationSchemes = AuthenticationConstants.Connector)]
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> Get([FromQuery] GetTxMediaQuery query, CancellationToken cancellationToken)
        {
            using (var operation = _telemetryClient.StartOperation<RequestTelemetry>("GetMediaQuerry-Connector"))
            {
                telemetryTrace.LogTrace("Get media image list for TX2 started", "Query", query, SeverityLevel.Information);
                var result = await mediator.Send(query, cancellationToken);
                telemetryTrace.LogTrace("Get media image list for TX2 list finished", "ResponseBody", result, SeverityLevel.Information);
                return Ok(result);
            }
        }

        [ServiceFilter(typeof(TenantFilter))]
        //[Authorize(AuthenticationSchemes = AuthenticationConstants.Connector)]
        [HttpPut]
        public async Task<IActionResult> Put(RenameImageMediaCommand command, CancellationToken cancellationToken)
        {
            using (var operation = _telemetryClient.StartOperation<RequestTelemetry>("RenameImageMedia-Connector"))
            {
                telemetryTrace.LogTrace("Rename image started", "RequestBody", command, SeverityLevel.Information);
                var result = await mediator.Send(command, cancellationToken);

                if (result.Success)
                {
                    telemetryTrace.LogTrace("Rename image finished for TX2 successfully", "ResponseBody", result, SeverityLevel.Information);
                    return Ok(result);
                }
                else
                {
                    telemetryTrace.LogTrace("Rename image finished for TX2 with errors", "ResponseBody", result, SeverityLevel.Information);
                    return BadRequest(result);
                }
            }
        }

    }
}
