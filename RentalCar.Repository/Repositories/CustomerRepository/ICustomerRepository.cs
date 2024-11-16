using RentalCar.Entity.Entities;
using RentalCar.Repository.Infrastructures;

namespace RentalCar.Repository.Repositories.CustomerRepository;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    public Customer? GetUserByEmail(string email);
    public bool CheckPasswordHash(Customer customer, string password);
    public Task<decimal> GetWalletBalance(Guid customerId);
    public Task<decimal> UpdateWalletBalance(Guid customerId, bool topUp, decimal amount);
}