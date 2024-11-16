using RentalCar.Entity.Entities;

namespace RentalCar.Repository.Repositories.AddressRepository
{
    public interface IAddressWardRepository
    {
        public Task<ICollection<AddressWard>> GetAllAddressWardOfDistrictAsync(Guid districtId);
    }
}
