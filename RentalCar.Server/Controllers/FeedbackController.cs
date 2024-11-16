using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalCar.Model;

namespace RentalCar.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IBookingService _bookingService;
        public FeedbackController(IFeedbackService feedbackService, IBookingService bookingService)
        {
            _feedbackService = feedbackService;
            _bookingService = bookingService;
        }

        // GET: api/Feedback
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackModel>>> GetFeedback()
        {
            var feedbacks = await _feedbackService.GetAllAsync();
            if (feedbacks == null)
                return NotFound(new { message = "Feedbacks not found" });
            return Ok(new { Feedbacks = feedbacks });
        }

        // GET: api/Feedback/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackModel>> GetFeedback(Guid id)
        {
            var feedback = await _feedbackService.GetByIdAsync(id);
            if (feedback == null)
            {
                return NotFound(new { message = "Feedback not found" });
            }

            return Ok(new { Feedback = feedback });
        }

        // POST: api/Feedback
        //[Authorize]
        [HttpPost]
        public async Task<ActionResult<FeedbackModel>> PostFeedback([FromBody] FeedbackModel feedbackModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid model state", errors = ModelState });
            }

            // Check if the booking already has feedback
            var booking = await _bookingService.GetByIdAsync(feedbackModel.BookingId);
            if (booking == null)
            {
                return NotFound(new { message = "Booking not found" });
            }

            if (booking.FeedbackId != null)
            {
                return BadRequest(new { message = "Feedback already exists for this booking." });
            }

            // Add the feedback
            await _feedbackService.AddAsync(feedbackModel);

            return CreatedAtAction(nameof(GetFeedback), new { id = feedbackModel.Id }, new { Feedback = feedbackModel, message = "Feedback created successfully" });
        }


        // PUT: api/Feedback/{id}
        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(Guid id, [FromBody] FeedbackModel feedbackModel)
        {
            if (id != feedbackModel.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            var feedbackToUpdate = await _feedbackService.GetByIdAsync(id);
            if (feedbackToUpdate == null)
            {
                return NotFound(new { message = "Feedback not found" });
            }

            await _feedbackService.UpdateAsync(feedbackModel);
            return Ok(new { message = "Feedback updated successfully" });
        }

        // DELETE: api/Feedback/{id}
        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            var feedbackToUpdate = await _feedbackService.GetByIdAsync(id);
            if (feedbackToUpdate == null)
            {
                return NotFound(new { message = "Feedback not found" });
            }

            await _feedbackService.DeleteAsync(id);
            return Ok(new { message = "Feedback deleted successfully" });
        }

        //[Authorize]
        [HttpGet("GetAverageRatings/{carOwnerId}")]
        public async Task<IActionResult> GetAverageRatings(Guid carOwnerId)
        {
            try
            {
                var averageRatings = await _feedbackService.GetAverageRatingsAsync(carOwnerId);

                if (averageRatings == 0)
                {
                    return NotFound(new { message = "No ratings available for the specified car owner." });
                }

                return Ok(new { averageRatings });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        //[Authorize]
        [HttpGet("GetNumbersOfRating/{carOwnerId}")]
        public async Task<IActionResult> GetNumbersOfRating(Guid carOwnerId)
        {
            try
            {
                // Retrieve the ratings for the specified car owner
                var ratings = (await _feedbackService.GetNumbersOfRatingAsync(carOwnerId)).ToList();

                // Check if the list is empty
                if (!ratings.Any())
                {
                    return NotFound(new { message = "No ratings found for the specified car owner." });
                }

                int star1 = ratings.ElementAtOrDefault(0);
                int star2 = ratings.ElementAtOrDefault(1);
                int star3 = ratings.ElementAtOrDefault(2);
                int star4 = ratings.ElementAtOrDefault(3);
                int star5 = ratings.ElementAtOrDefault(4);
                int starAll = star1 + star2 + star3 + star4 + star5;
                // Prepare the response
                return Ok(new
                {
                    StarAll = starAll,
                    Star1 = star1,
                    Star2 = star2,
                    Star3 = star3,
                    Star4 = star4,
                    Star5 = star5
                });
            }
            catch (Exception ex)
            {
                // Log the exception as necessary
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }


        //[Authorize]
        [HttpGet("GetListFeedbacks/{carOwnerId}")]
        public async Task<IActionResult> GetListFeedbacks(Guid carOwnerId, [FromQuery] int star, [FromQuery] int index, [FromQuery] int feedbackPerPage)
        {
            try
            {
                var (feedbacks, totalPages) = await _feedbackService.GetListFeedbacksAsync(carOwnerId, star, index, feedbackPerPage);
                return Ok(new { feedbacks, totalPages });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
