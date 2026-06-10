using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMedia.Api.Constants;
using Services.Command.ImageMedia;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Logging;
using txc_common_lib.Filters.Models;

namespace ServiceMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TxImageBlobController : ApiBaseController
    {
        private readonly ITelemetryLogTrace<TxImageBlobController> telemetryTrace;
        private readonly TelemetryClient _telemetryClient;
        public TxImageBlobController(IMediator mediator
            , ITelemetryLogTrace<TxImageBlobController> telemetryTrace
            , TelemetryClient telemetryClient) : base(mediator)
        {
            this.telemetryTrace = telemetryTrace;
            _telemetryClient = telemetryClient;
        }

        [ServiceFilter(typeof(TenantFilter))]
        //[Authorize(AuthenticationSchemes = AuthenticationConstants.Connector)]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Post([FromForm] CreateImageMediaCommand command, CancellationToken cancellationToken)
        {
            using (var operation = _telemetryClient.StartOperation<RequestTelemetry>("CreateImageMedia-Connector"))
            {
                telemetryTrace.LogTrace("Create media image started", "RequestBody", command, SeverityLevel.Information);
                var result = await mediator.Send(command, cancellationToken);

                if (result.Success)
                {
                    telemetryTrace.LogTrace("Create media image finished successfully", "ResponseBody", result, SeverityLevel.Information);
                    return Ok(result);
                }
                else
                {
                    telemetryTrace.LogTrace("Create media image finished with errors", "ResponseBody", result, SeverityLevel.Information);
                    return BadRequest(result);
                }
            }
        }

        [ServiceFilter(typeof(TenantFilter))]
        //[Authorize(AuthenticationSchemes = AuthenticationConstants.Connector)]
        [HttpPut, DisableRequestSizeLimit]
        public async Task<IActionResult> Put([FromForm] ReplaceImageMediaCommand command, CancellationToken cancellationToken)
        {
            using (var operation = _telemetryClient.StartOperation<RequestTelemetry>("ReplaceImageMediaBlob-Connector"))
            {
                telemetryTrace.LogTrace("Replace media image started", "RequestBody", command, SeverityLevel.Information);
                var result = await mediator.Send(command, cancellationToken);

                if (result.Success)
                {
                    telemetryTrace.LogTrace("Replace media image finished successfully", "ResponseBody", result, SeverityLevel.Information);
                    return Ok(result);
                }
                else
                {
                    telemetryTrace.LogTrace("Replace media image finished with errors", "ResponseBody", result, SeverityLevel.Information);
                    return Ok(result);
                }
            }
        }

    }
}
