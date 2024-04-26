using IMDB.CustomExceptions;
using IMDB.Models.Request;
using IMDB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMDB.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ReviewRequest review)
        {
            int id;
            try
            {
                id = await _reviewService.CreateAsync(review);
                review.Id = id;
                return CreatedAtAction(nameof(GetAsync), new { Id = id }, review);
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

        [HttpGet("/movies/{movieId}/reviews")]
        public async Task<IActionResult> GetAsync([FromRoute]int movieId)
        {
            try
            {
                return Ok(await _reviewService.GetAsync(movieId));
            }
            catch(IdNotExistException ex)
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


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                return Ok(await _reviewService.GetByIdAsync(id));
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
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] ReviewRequest review)
        {
            try
            {
                review.Id = id;
                return Ok(await _reviewService.UpdateAsync(review));
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
                return Ok(await _reviewService.DeleteAsync(id));
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
    }
}
