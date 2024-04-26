using IMDB.CustomExceptions;
using IMDB.Models.Database;
using IMDB.Models.Request;
using IMDB.Models.Response;
using IMDB.Services;
using IMDB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*
 * Controller should have proper methods to handle following operation with usage of HTTP Verbs
    - Create - POST /resources
    - Get all - GET /resources
    - Get by Id - GET /resources/{resourceId}
    - Update - PUT /resources/{resourceId}
    - Delete - DELETE /resources/{resourceId}
*/
namespace IMDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] MovieRequest movie)
        {
            try
            {
                int id = await _movieService.CreateAsync(movie);
                movie.Id = id;
                return CreatedAtAction(nameof(GetAsync), new { Id = id }, movie);
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
        public async Task<IActionResult> GetAsync([FromQuery] int year=-1)
        {
            try
            {
                List<MovieResponse> result = new();
                if (year == 0)
                    return BadRequest();
                else if (year != -1)
                    result = await _movieService.GetByYearAsync(year);
                else
                    result = await _movieService.GetAsync();

                return Ok(result);
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var result = await _movieService.GetAsync(id);
                return Ok(result);
            }
            catch (IdNotExistException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ErrorInRepositoryException ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] MovieRequest movie)
        {
            try
            {
                movie.Id = id;
                return Ok(await _movieService.UpdateAsync(movie));
            }
            catch (IdNotExistException ex)
            {
                return BadRequest(ex.Message);
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
                return Ok(await _movieService.DeleteAsync(id));
            }
            catch (IdNotExistException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ErrorInRepositoryException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
