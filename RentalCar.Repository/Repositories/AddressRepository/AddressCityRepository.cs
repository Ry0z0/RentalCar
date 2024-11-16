using Microsoft.EntityFrameworkCore;
using RentalCar.Entity.Contexts;
using RentalCar.Entity.Entities;

namespace RentalCar.Repository.Repositories.AddressRepository
{
    public class AddressCityRepository : IAddressCityRepository
    {
        private readonly RentalCarDbContext _context;

        public AddressCityRepository(RentalCarDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<AddressCity>> GetAllAddressCityAsync()
        {
            var listcity = await _context.AddressCities.ToListAsync();
            return listcity;
        }
    }
}