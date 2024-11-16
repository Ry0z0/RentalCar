using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalCar.Model;
using RentalCar.Repository.Repositories.CarRepositoryRepository;

namespace RentalCar.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: api/Booking
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BookingModel>>> GetBookings()
        {
            var bookings = await _bookingService.GetAllAsync();
            if (bookings == null)
                return NotFound(new { message = "Bookings not found" });
            return Ok(new { Bookings = bookings });
        }

        // GET: api/Booking/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingModel>> GetBooking(Guid id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound(new { message = "Booking not found" });
            }

            return Ok(new { Booking = booking });
        }

        // POST: api/Booking
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BookingModel>> PostBooking([FromBody] BookingModel bookingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid model state", errors = ModelState });
            }

            await _bookingService.AddAsync(bookingModel);
            return CreatedAtAction(nameof(GetBooking), new { id = bookingModel.Id },
                new { Booking = bookingModel, message = "Booking created successfully" });
        }

        // PUT: api/Booking/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(Guid id, [FromBody] BookingModel bookingModel)
        {
            if (id != bookingModel.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            var bookingToUpdate = await _bookingService.GetByIdAsync(id);
            if (bookingToUpdate == null)
            {
                return NotFound(new { message = "Booking not found" });
            }

            await _bookingService.UpdateAsync(bookingModel);
            return Ok(new { message = "Booking updated successfully" });
        }

        // DELETE: api/Booking/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            var bookingToUpdate = await _bookingService.GetByIdAsync(id);
            if (bookingToUpdate == null)
            {
                return NotFound(new { message = "Booking not found" });
            }

            await _bookingService.DeleteAsync(id);
            return Ok(new { message = "Booking deleted successfully" });
        }

        [Authorize]
        [HttpGet("GetNumberOfBookingOfCar/{id}")]
        public async Task<IActionResult> GetNumberOfBookingOfCar(Guid id)
        {
            var number = await _bookingService.GetNumberOfBookingOfCarAsync(id);
            return Ok(new { numberOfBooking = number });
        }

        [Authorize]
        [HttpGet("GetLatestBookingOfCustomer/{customerId}")]
        public async Task<IActionResult> GetLatestBookingOfCustomer(Guid customerId)
        {
            var booking = await _bookingService.GetLatestBookingOfCustomerAsync(customerId);
            return Ok(new { booking = booking });
        }

        /*
        [Authorize]
        */
        [HttpGet("GetLastBooking")]
        public async Task<IActionResult> GetLastBooking([FromHeader] DateTime date)
        {
            var booking = await _bookingService.GetLastBookingOfTheDayAsync(date);
            return Ok(new { booking = booking });
        }

        [HttpGet("GetBookingNo")]
        public async Task<IActionResult> GetBookingNo([FromQuery] DateTime date)
        {
            var bookingNo = await _bookingService.GenerateBookingNoAsync(date);
            return Ok(new { bookingNo = bookingNo });
        }

        [HttpGet("GetListBookingCar/{CustomerId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<IEnumerable<BookingCarDTO>>> GetListBookingCar(Guid CustomerId)
        {
            var bookingcars = await _bookingService.GetListBookingCar(CustomerId);
            if (bookingcars == null)
                return NotFound(new { message = "BookingCars not found" });
            return Ok(new { Bookingcars = bookingcars });
        }

        [Authorize]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateBookingStatus(Guid id, [FromBody] PatchResponse status)
        {

            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound(new { message = "Booking not found" });
            }

            booking.Status = status.status;

            try
            {
                await _bookingService.UpdateAsync(booking);
                return Ok(new { message = "Booking status updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the booking status." });
            }
        }
    }
    public class PatchResponse
    {
        public string status { get; set; }
    }
}