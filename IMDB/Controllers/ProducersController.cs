using IMDB.CustomExceptions;
using IMDB.Models.Request;
using IMDB.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMDB.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProducersController : ControllerBase
    {
        private readonly IProducerService _producerService;

        public ProducersController(IProducerService producerService)
        {
            _producerService = producerService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ProducerRequest producer)
        {
            try
            {
                int id = await _producerService.CreateAsync(producer);
                producer.Id = id;
                return CreatedAtAction(nameof(GetAsync), new { Id = id }, producer);
            }
            catch (InvalidRequestObjectException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ErrorInRepositoryException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                return Ok(await _producerService.GetAsync());
            }
            catch (NoItemFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ErrorInRepositoryException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            try
            {
                var result = await _producerService.GetAsync(id);
                return Ok(result);
            }
            catch (IdNotExistException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NoItemFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ErrorInRepositoryException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] ProducerRequest producer)
        {
            try
            {
                producer.Id = id;
                return Ok(await _producerService.UpdateAsync(producer));
            }
            catch (IdNotExistException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidRequestObjectException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ErrorInRepositoryException ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                return Ok(await _producerService.DeleteAsync(id));
            }
            catch (IdNotExistException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ErrorInRepositoryException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
