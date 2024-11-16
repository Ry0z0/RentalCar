using Microsoft.EntityFrameworkCore;
using RentalCar.Entity.Contexts;
using RentalCar.Entity.Entities;

namespace RentalCar.Repository.Repositories.AddressRepository
{
    public class AddressWardRepository : IAddressWardRepository
    {
        private readonly RentalCarDbContext _context;

        public AddressWardRepository(RentalCarDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<AddressWard>> GetAllAddressWardOfDistrictAsync(Guid districtId)
        {
            return await _context.AddressWards.Where(x => x.DistrictId == districtId).ToListAsync();
        }
    }
}
