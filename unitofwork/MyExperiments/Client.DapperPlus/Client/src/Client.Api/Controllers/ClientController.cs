using Client.Api.Constants;
using Client.Api.Controllers;
using Client.Api.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Command.Client;
using Services.Queries.Client;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ApiBaseController
    {
        public ClientController(IMediator mediator) : base(mediator)
        {
        }

        //[Authorize(AuthenticationSchemes = AuthenticationConstants.DefaultScheme)]
        [ServiceFilter(typeof(TenantFilter))]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetClientListQuery query, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        //[Authorize(AuthenticationSchemes = AuthenticationConstants.DefaultScheme)]
        [ServiceFilter(typeof(TenantFilter))]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Post([FromBody] CreateClientCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);

            if (result.Success)
            {
                return Ok(new ResponseMessageDto
                {
                    Message = "Success",
                    Code = 200
                });
            }
            else
            {
                return BadRequest(new ResponseMessageDto
                {
                    Message = result.Message,
                    Code = 400
                });
            }

        }

        //[Authorize(AuthenticationSchemes = AuthenticationConstants.DefaultScheme)]
        [ServiceFilter(typeof(TenantFilter))]
        [HttpPut, DisableRequestSizeLimit]
        public async Task<IActionResult> Put([FromBody] UpdateClientCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);

            if (result.Success)
            {
                return Ok(new ResponseMessageDto
                {
                    Message = "Success",
                    Code = 200
                });
            }
            else
            {
                return BadRequest(new ResponseMessageDto
                {
                    Message = result.Message,
                    Code = 400
                });
            }
        }

    }
}
