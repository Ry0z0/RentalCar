using Microsoft.EntityFrameworkCore;
using RentalCar.Entity.Contexts;
using RentalCar.Entity.Entities;

namespace RentalCar.Repository.Repositories.AddressRepository
{
    public class AddressDistrictRepository : IAddressDistrictRepository
    {
        private readonly RentalCarDbContext _context;

        public AddressDistrictRepository(RentalCarDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<AddressDistrict>> GetAllAddressDistrictOfCityAsync(Guid cityId)
        {
            return await _context.AddressDistricts.Where(x => x.CityId == cityId).ToListAsync();
        }
    }
}
