using Domain.Dto.Request;
using Domain.Dto.Response;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//====EF
using static RepositoryEf.UnitOfWork.MediaUnit;

//////====Dapper
//using static RepositoryDapper.UnitOfWork.MediaUnit;

namespace RepositoryPatternHybrid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaUsingSqlController : ControllerBase
    {
        private IMediaUnitOfWork _media;
        public MediaUsingSqlController(IMediaUnitOfWork media,
            IConfiguration configuration)
        {
            media.SetConnection(configuration["ConnectionStrings:MediaConnectionString"]);
            _media = media;
        }



        [HttpGet("GetMediaById")]
        public async Task<MediaResponseDto> GetMediaById([FromQuery] MediaRequestDto request)
        {
            var result = await _media.MediaRepository.GetMediaById(request);
            return result;
        }
    }
}
