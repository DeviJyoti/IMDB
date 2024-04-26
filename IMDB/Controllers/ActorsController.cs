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
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;

        public ActorsController(IActorService actorService)
        {
            _actorService = actorService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ActorRequest actor)
        {
            try
            {
                int id = await _actorService.CreateAsync(actor);
                actor.Id = id;
                return CreatedAtAction(nameof(GetAsync), new { Id = id }, actor);
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
                return Ok(await _actorService.GetAsync());
            }
            catch(NoItemFoundException ex)
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
                var result = await _actorService.GetAsync(id);
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
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] ActorRequest actor)
        {
            try
            {
                actor.Id = id;
                return Ok(await _actorService.UpdateAsync(actor));
            }
            catch(IdNotExistException ex)
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
                return Ok(await _actorService.DeleteAsync(id));
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
