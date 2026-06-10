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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageMediaBlobController : ApiBaseController
    {
        private readonly ITelemetryLogTrace<ImageMediaBlobController> telemetryTrace;
        private readonly TelemetryClient _telemetryClient;
        public ImageMediaBlobController(IMediator mediator
            , ITelemetryLogTrace<ImageMediaBlobController> telemetryTrace
            , TelemetryClient telemetryClient) : base(mediator)
        {
            this.telemetryTrace = telemetryTrace;
            _telemetryClient = telemetryClient;
        }
        [Authorize(AuthenticationSchemes = AuthenticationConstants.AllAuthScheme)]
        [ServiceFilter(typeof(TenantFilter))]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Post([FromForm] CreateImageMediaCommand command, CancellationToken cancellationToken)
        {
            using (var operation = _telemetryClient.StartOperation<RequestTelemetry>("CreateImageMedia"))
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
        
        [Authorize(AuthenticationSchemes = AuthenticationConstants.AllAuthScheme)]
        [ServiceFilter(typeof(TenantFilter))]
        [HttpPut, DisableRequestSizeLimit]
        public async Task<IActionResult> Put([FromForm] ReplaceImageMediaCommand command, CancellationToken cancellationToken)
        {
            using (var operation = _telemetryClient.StartOperation<RequestTelemetry>("ReplaceImageMediaBlob"))
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
                    return BadRequest(result);
                }
            }
        }

        [Authorize(AuthenticationSchemes = AuthenticationConstants.AllAuthScheme)]
        [ServiceFilter(typeof(TenantFilter))]
        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteImageMediaCommand command, CancellationToken cancellationToken)
        {
            using (var operation = _telemetryClient.StartOperation<RequestTelemetry>("DeleteImageMediaBlob"))
            {
                telemetryTrace.LogTrace("Delete media image started", "RequestBody", command, SeverityLevel.Information);
                var result = await mediator.Send(command, cancellationToken);

                if (result.Success)
                {
                    telemetryTrace.LogTrace("Delete media image finished successfully", "ResponseBody", result, SeverityLevel.Information);
                    return Ok(result);
                }
                else
                {
                    telemetryTrace.LogTrace("Delete media image finished with errors", "ResponseBody", result, SeverityLevel.Information);
                    return BadRequest(result);
                }
            }
        }
    }
}
