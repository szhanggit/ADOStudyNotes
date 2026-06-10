using Microsoft.AspNetCore.Mvc;
using Service.Protos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RepositoryPatternHybrid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly Products.ProductsClient _cli;
        public ProductsController(Products.ProductsClient cli)
        {
            _cli = cli;
        }
       
        // POST api/<ProductsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UpsertProductRequest request)
        {
            return Ok(await _cli.CreateAsync(request));
        }

        // PUT api/<ProductsController>/5
        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] UpsertProductRequest request)
        {
            return Ok(await _cli.UpdateAsync(request));
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _cli.RemoveAsync(new RemoveProductRequest { Id = id }));
        }
    }
}
