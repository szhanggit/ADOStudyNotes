using Domain.Dto;
using Domain.Dto.Request;
using Domain.Dto.Response;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

//====EF
using static RepositoryEf.UnitOfWork.MediaUnit;

//////====Dapper
//using static RepositoryDapper.UnitOfWork.MediaUnit;



namespace RepositoryPatternHybrid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private IMediaUnitOfWork _media;
        public MediaController(IMediaUnitOfWork media,
            IConfiguration configuration)
        {
            media.SetConnection(configuration["ConnectionStrings:MediaConnectionString"]);
            _media = media;
        }
        // GET: api/<MediaController>
        [HttpGet]
        public async Task<IEnumerable<Media>> Get()
        {
            return await _media.MediaRepository.Get();
        }

        [HttpGet("{id}")]
        public async Task<Media> Get(int id)
        {
            var result = await _media.MediaRepository.GetMediaById(id);
            return result;
        }

        // POST api/<MediaController>
        [HttpPost]
        public async Task<int> Post([FromBody] Media value)
        {
            await _media.MediaRepository.Add(value);
            //await _media.MediaRepository.CustomAdd(value);
            var result = await _media.Complete();
            return result;
        }

        // PUT api/<MediaController>/5
        [HttpPut]
        public async Task<int> Put([FromBody] Media value)
        {
            await _media.MediaRepository.Update(value);
            var result = await _media.Complete();
            return result;
        }

        // DELETE api/<MediaController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var entity = await _media.MediaRepository.GetMediaById(id);
            if (entity == null)
                throw new Exception("Record not found");
            await _media.MediaRepository.Remove(entity);
            await _media.Complete();
        }
    }
}
