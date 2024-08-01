using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MIS_Healthcare.API.Data.Models;
using MIS_Healthcare.API.Middleware;
using MIS_Healthcare.API.Repository.Interface;

namespace MIS_Healthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly iFeedbackRepo _feedbackRepository;

        public FeedbacksController(iFeedbackRepo feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFeedback()
        {
            try
            {
                var feedback = await _feedbackRepository.GetAllFeedbackAsync();
                return Ok(feedback);
            }
            catch (RepositoryException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching feedback.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeedbackById(int id)
        {
            try
            {
                var feedback = await _feedbackRepository.GetFeedbackByIdAsync(id);
                if (feedback == null)
                {
                    return NotFound();
                }
                return Ok(feedback);
            }
            catch (RepositoryException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the feedback.", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFeedback([FromBody] Feedback feedback)
        {
            try
            {
                await _feedbackRepository.AddFeedbackAsync(feedback);
                return CreatedAtAction(nameof(GetFeedbackById), new { id = feedback.FeedbackID }, feedback);
            }
            catch (RepositoryException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the feedback.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, [FromBody] Feedback feedback)
        {
            if (id != feedback.FeedbackID)
            {
                return BadRequest();
            }

            try
            {
                await _feedbackRepository.UpdateFeedbackAsync(feedback);
                return NoContent();
            }
            catch (RepositoryException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the feedback.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            try
            {
                await _feedbackRepository.DeleteFeedbackAsync(id);
                return NoContent();
            }
            catch (RepositoryException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the feedback.", details = ex.Message });
            }
        }
    }
}
