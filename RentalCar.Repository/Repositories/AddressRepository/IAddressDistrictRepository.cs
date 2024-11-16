using RentalCar.Entity.Entities;

namespace RentalCar.Repository.Repositories.AddressRepository
{
    public interface IAddressDistrictRepository
    {
        public Task<ICollection<AddressDistrict>> GetAllAddressDistrictOfCityAsync(Guid cityId);
    }
}
