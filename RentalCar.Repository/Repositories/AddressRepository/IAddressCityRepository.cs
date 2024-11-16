using RentalCar.Entity.Entities;

namespace RentalCar.Repository.Repositories.AddressRepository
{
    public interface IAddressCityRepository
    {
        public Task<ICollection<AddressCity>> GetAllAddressCityAsync();
    }
}
