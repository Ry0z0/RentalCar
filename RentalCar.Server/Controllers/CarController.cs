using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalCar.Model;

namespace RentalCar.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        // GET: api/Car
        [Authorize(Roles = "CarOwner")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarModel>>> GetCars()
        {
            var cars = await _carService.GetAllAsync();
            if (cars == null)
                return NotFound(new { message = "Cars not found" });
            return Ok(new { Cars = cars });
        }

        // GET: api/Car/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CarModel>> GetCar(Guid id)
        {
            var car = await _carService.GetByIdAsync(id);
            if (car == null)
            {
                return NotFound(new { message = "Car not found" });
            }

            return Ok(new { Car = car });
        }

        [Authorize]
        [HttpGet("GetCarNameOfBooking/{bookingId}")]
        public async Task<ActionResult<CarModel>> GetCarNameOfBooking(Guid bookingId)
        {
            var name = await _carService.GetCarNameInBooking(bookingId);
            return Ok(new
            {
                carName = name,
            });
        }

        // POST: api/Car
        [Authorize(Roles = "CarOwner")]
        [HttpPost]
        public async Task<ActionResult<CarModel>> PostCar([FromBody] CarModel carModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid model state", errors = ModelState });
            }

            await _carService.AddAsync(carModel);
            return CreatedAtAction(nameof(GetCar), new { id = carModel.Id }, new { Car = carModel, message = "Car created successfully" });
        }

        // PUT: api/Car/{id}
        //[Authorize(Roles = "CarOwner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(Guid id, [FromBody] CarModel carModel)
        {
            if (id != carModel.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            var carToUpdate = await _carService.GetByIdAsync(id);
            if (carToUpdate == null)
            {
                return NotFound(new { message = "Car not found" });
            }

            await _carService.UpdateAsync(carModel);
            return Ok(new { message = "Car updated successfully" });
        }

        // DELETE: api/Car/{id}
        [Authorize(Roles = "CarOwner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(Guid id)
        {
            var carToDelete = await _carService.GetByIdAsync(id);
            if (carToDelete == null)
            {
                return NotFound(new { message = "Car not found" });
            }

            await _carService.DeleteAsync(id);
            return NoContent(); 
        }

        // SEARCH: api/Car/Search
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<CarModel>>> SearchCar([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string location)
        {
            try
            {
                var cars = await _carService.SearchCarAsync(startDate, endDate, location);
                if (cars == null || !cars.Any())
                {
                    return NotFound(new { message = "No cars available for the given criteria" });
                }
                return Ok(new { Cars = cars });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // SEARCH BY OWNER: api/Car/SearchByOwner/{carOwnerId}
        [HttpGet("SearchByOwner/{carOwnerId}")]
        public async Task<ActionResult<IEnumerable<SearchCarDTO>>> SearchCarByOwner(Guid carOwnerId)
        {
            try
            {
                var cars = await _carService.SearchCarByOwnerAsync(carOwnerId);
                if (cars == null || !cars.Any())
                {
                    return NotFound(new { message = "No cars available for the given owner." });
                }
                return Ok(new { Cars = cars });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET STATUS BY OWNER: api/Car/GetBookingStatus/{carOwnerId}
        [HttpGet("GetBookingStatus/{carOwnerId}")]
        public async Task<ActionResult<IEnumerable<CarStatusDTO>>> GetBookingStatusByOwner(Guid carOwnerId)
        {
            try
            {
                var carStatuses = await _carService.GetCarStatusesForOwnerAsync(carOwnerId);
                if (carStatuses == null || !carStatuses.Any())
                {
                    return NotFound(new { message = "No cars available for the given owner." });
                }
                return Ok(new { CarStatuses = carStatuses });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
