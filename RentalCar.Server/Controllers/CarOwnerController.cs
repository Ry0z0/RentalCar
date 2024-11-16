using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalCar.Entity.Contexts;
using RentalCar.Model;

namespace RentalCar.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarOwnerController : ControllerBase
    {
        private readonly ICarOwnerService _carOwnerService;
        private readonly RentalCarDbContext _dbContext;
        public CarOwnerController(ICarOwnerService carOwnerService, RentalCarDbContext dbContext)
        {
            _carOwnerService = carOwnerService;
            _dbContext = dbContext;
        }

        // GET: api/CarOwner
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarOwnerModel>>> GetCarOwner()
        {
            var carOwners = await _carOwnerService.GetAllAsync();
            if (carOwners == null)
                return NotFound(new { message = "Car owners not found" });
            return Ok(new { CarOwners = carOwners });
        }

        // GET: api/CarOwner/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CarOwnerModel>> GetCarOwner(Guid id)
        {
            var carOwner = await _carOwnerService.GetByIdAsync(id);
            if (carOwner == null)
            {
                return NotFound(new { message = "Car owner not found" });
            }
            return Ok(new { CarOwner = carOwner });
        }

        // POST: api/CarOwner
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CarOwnerModel>> PostCarOwner([FromBody] CarOwnerModel carOwnerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid model state", errors = ModelState });
            }

            await _carOwnerService.AddAsync(carOwnerModel);
            return CreatedAtAction(nameof(GetCarOwner), new { id = carOwnerModel.Id }, new { CarOwner = carOwnerModel, message = "Car owner created successfully" });
        }

        // PUT: api/CarOwner/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task PutCarOwner(CarOwnerModel carOwnerModel)
        {
            var existingCarOwner = await _dbContext.CarOwners.FindAsync(carOwnerModel.Id);

            if (existingCarOwner == null)
            {
                throw new InvalidOperationException("CarOwner not found");
            }

            // Cập nhật các thuộc tính của existingCustomer bằng giá trị từ customerModel
            _dbContext.Entry(existingCarOwner).CurrentValues.SetValues(carOwnerModel);

            await _dbContext.SaveChangesAsync();
        }

        // DELETE: api/CarOwner/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarOwner(Guid id)
        {
            var carOwnerToDelete = await _carOwnerService.GetByIdAsync(id);
            if (carOwnerToDelete == null)
            {
                return NotFound(new { message = "Car owner not found" });
            }

            await _carOwnerService.DeleteAsync(id);
            return Ok(new { message = "Car owner deleted successfully" });
        }
    }
}
