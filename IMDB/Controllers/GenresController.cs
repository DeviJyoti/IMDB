using IMDB.CustomExceptions;
using IMDB.Models.Request;
using IMDB.Services;
using IMDB.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMDB.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] GenreRequest genre)
        {
            try
            {
                int id = await _genreService.CreateAsync(genre);
                genre.Id = id;
                return CreatedAtAction(nameof(GetAsync), new { Id = id }, genre);
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
                return Ok(await _genreService.GetAsync());
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
                var result = await _genreService.GetAsync(id);
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
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] GenreRequest genre)
        {
            try
            {
                genre.Id = id;
                return Ok(await _genreService.UpdateAsync(genre));
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
                return Ok(await _genreService.DeleteAsync(id));
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
