using RentalCar.Entity.Contexts;
using RentalCar.Entity.Entities;
using RentalCar.Repository.Infrastructures;

namespace RentalCar.Repository.Repositories.CustomerRepository
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        private readonly RentalCarDbContext _context;

        public CustomerRepository(RentalCarDbContext context) : base(context)
        {
            _context = context;
        }

        public Customer? GetUserByEmail(string email)
        {
            var user = _context.Customers.FirstOrDefault(e => e.Email == email);
            return user;
        }

        public bool CheckPasswordHash(Customer customer, string password)
        {
            var passwordHash = HashPassword(password);
            return customer.PasswordHash == passwordHash;
        }

        public Task<decimal> GetWalletBalance(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> UpdateWalletBalance(Guid customerId, bool topUp, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
