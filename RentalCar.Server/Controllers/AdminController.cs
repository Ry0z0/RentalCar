using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalCar.Entity.Entities;
using RentalCar.Model;

namespace RentalCar.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ICarOwnerService _carOwnerService;
        private readonly ICarService _carService;
        private readonly IMapper _mapper;

        public AdminController(ICustomerService customerService, ICarOwnerService carOwnerService, ICarService carService, IMapper mapper)
        {
            _customerService = customerService;
            _carOwnerService = carOwnerService;
            _carService = carService;
            _mapper = mapper;
        }

        // GET: api/Admin/Users
        [HttpGet("Users")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllUsers()
        {
            var customers = await _customerService.GetAllAsync();
            var carOwners = await _carOwnerService.GetAllAsync();

            var users = customers.Cast<object>().Concat(carOwners.Cast<object>());

            return Ok(new { Users = users });
        }

        [HttpGet("SearchUsers")]
        public async Task<ActionResult<IEnumerable<object>>> SearchUsers(string query)
        {
            var customers = await _customerService.GetAllAsync();
            var carOwners = await _carOwnerService.GetAllAsync();

            var users = customers.Cast<object>().Concat(carOwners.Cast<object>());

            var result = users.Where(user =>
                user.GetType().GetProperty("Id")?.GetValue(user)?.ToString().Contains(query, StringComparison.OrdinalIgnoreCase) == true ||
                user.GetType().GetProperty("Name")?.GetValue(user)?.ToString().Contains(query, StringComparison.OrdinalIgnoreCase) == true ||
                user.GetType().GetProperty("Email")?.GetValue(user)?.ToString().Contains(query, StringComparison.OrdinalIgnoreCase) == true
            );

            return Ok(new { Users = result });
        }


        // GET: api/Admin/Users/{type}
        [HttpGet("Users/{type}")]
        public async Task<ActionResult<IEnumerable<object>>> GetUsersByType(string type)
        {
            if (type.Equals("customer", StringComparison.OrdinalIgnoreCase))
            {
                var customers = await _customerService.GetAllAsync();
                return Ok(new { Users = customers });
            }
            else if (type.Equals("carowner", StringComparison.OrdinalIgnoreCase))
            {
                var carOwners = await _carOwnerService.GetAllAsync();
                return Ok(new { Users = carOwners });
            }
            else
            {
                return BadRequest(new { message = "Invalid user type. Valid types are 'customer' and 'carowner'." });
            }
        }

        [HttpDelete("Users/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer != null)
            {
                await _customerService.DeleteAsync(id);
                return Ok(new { message = "Customer deleted successfully." });
            }

            var carOwner = await _carOwnerService.GetByIdAsync(id);
            if (carOwner != null)
            {
                await _carOwnerService.DeleteAsync(id);
                return Ok(new { message = "Car owner deleted successfully." });
            }

            return NotFound(new { message = "User not found." });
        }

        [HttpPut("UpdateUserAdmin")]
        public async Task<IActionResult> UpdateUserAdmin([FromBody] UserDetailDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid model state", errors = ModelState });
            }

            try
            {
                // Determine if the user is a Customer or CarOwner
                if (userDto.Role == "customer")
                {
                    var customerModel = _mapper.Map<CustomerModel>(userDto);

                    // Update the customer using the service
                    var updatedCustomer = await _customerService.UpdateAsync(customerModel);

                    if (updatedCustomer == null)
                    {
                        return NotFound(new { message = "Customer not found or could not be updated." });
                    }

                    return Ok(new { message = "Customer updated successfully", User = updatedCustomer });
                }
                else if (userDto.Role == "carowner")
                {
                    var carOwnerModel = _mapper.Map<CarOwnerModel>(userDto);

                    // Update the car owner using the service
                    var updatedCarOwner = await _carOwnerService.UpdateAsync(carOwnerModel);

                    if (updatedCarOwner == null)
                    {
                        return NotFound(new { message = "Car owner not found or could not be updated." });
                    }

                    return Ok(new { message = "Car owner updated successfully", User = updatedCarOwner });
                }
                else
                {
                    return BadRequest(new { message = "Invalid role specified. Valid roles are 'customer' and 'carowner'." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        //Car

        // GET ALL CARS FOR ADMIN: api/Car/GetAllCarToAdmin
        [HttpGet("GetAllCarToAdmin")]
        public async Task<ActionResult<IEnumerable<SearchCarDTO>>> GetAllCarToAdmin()
        {
            try
            {
                var cars = await _carService.GetAllCarsForAdminAsync();
                if (cars == null || !cars.Any())
                {
                    return NotFound(new { message = "No cars available." });
                }
                return Ok(new { Cars = cars });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // UPDATE: api/Car/UpdateCarAdmin
        [HttpPut("UpdateCarAdmin")]
        public async Task<IActionResult> UpdateCarAdmin([FromBody] SearchCarDTO carDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid model state", errors = ModelState });
            }

            try
            {
                // Map SearchCarDTO to Car entity
                var carEntity = _mapper.Map<Car>(carDto);

                // Convert back to CarModel for service layer
                var carModel = _mapper.Map<CarModel>(carEntity);

                // Update the car using the service
                var updatedCar = await _carService.UpdateAsync(carModel);

                if (updatedCar == null)
                {
                    return NotFound(new { message = "Car not found or could not be updated." });
                }

                return Ok(new { message = "Car updated successfully", Car = updatedCar });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


    }
}