using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalCar.Entity.Contexts;
using RentalCar.Model;

namespace RentalCar.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly RentalCarDbContext _dbContext;

        public CustomerController(ICustomerService customerService, RentalCarDbContext dbcontext)
        {
            _customerService = customerService;
            _dbContext = dbcontext;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetCustomers()
        {
            var customers = await _customerService.GetAllAsync();
            if (customers == null)
                return NotFound(new { message = "Customers not found" });
            return Ok(new { Customers = customers });
        }

        // GET: api/Customer/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerModel>> GetCustomer(Guid id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound(new { message = "Customer not found" });
            }
            return Ok(new { Customer = customer });
        }

        // POST: api/Customer
        //[Authorize]
        [HttpPost]
        public async Task<ActionResult<CustomerModel>> PostCustomer([FromBody] CustomerModel customerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid model state", errors = ModelState });
            }

            await _customerService.AddAsync(customerModel);
            return CreatedAtAction(nameof(GetCustomer), new { id = customerModel.Id }, new { Customer = customerModel, message = "Customer created successfully" });
        }

        // PUT: api/Customer/{id}
        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(CustomerModel customerModel)
        {
            var existingCustomer = await _customerService.GetByIdAsync(customerModel.Id);

            if (existingCustomer == null)
            {
                throw new InvalidOperationException("Customer not found");
            }

            await _customerService.UpdateAsync(customerModel);
            return Ok(new { message = "Customer updated successfully" });
        }

        // DELETE: api/Customer/{id}
        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customerToDelete = await _customerService.GetByIdAsync(id);
            if (customerToDelete == null)
            {
                return NotFound(new { message = "Customer not found" });
            }

            await _customerService.DeleteAsync(id);
            return Ok(new { message = "Customer deleted successfully" });
        }
        // PATCH: api/Customer/{id}/wallet
        [Authorize]
        [HttpPatch("{id}/wallet")]
        public async Task<IActionResult> PatchCustomerWallet(Guid id, [FromBody] CustomerWalletPatchRequest wallet)
        {
            // Kiểm tra nếu giá trị wallet không hợp lệ
            if (string.IsNullOrEmpty(wallet.wallet) || wallet.wallet.Length > 100)
            {
                return BadRequest(new { message = "Invalid wallet value. It should not be empty and must be less than 100 characters." });
            }

            var customer = await _dbContext.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound(new { message = "Customer not found" });
            }

            // Cập nhật giá trị số dư của ví
            customer.Wallet = wallet.wallet;

            // Lưu thay đổi vào cơ sở dữ liệu
            _dbContext.Entry(customer).Property(c => c.Wallet).IsModified = true;
            await _dbContext.SaveChangesAsync();

            return Ok(new { Wallet = customer.Wallet, message = "Wallet updated successfully" });
        }

        public class CustomerWalletPatchRequest
        {
            public string? wallet { get; set; } // Kiểu dữ liệu string cho wallet
        }


        // GET: api/Customer/{id}/wallet
        [Authorize]
        [HttpGet("GetCustomerWallet/{id}")]
        public async Task<IActionResult> GetCustomerWallet(Guid id)
        {
            var customer = await _dbContext.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound(new { message = "Customer not found" });
            }

            return Ok(new { Wallet = customer.Wallet });
        }
    }
}
