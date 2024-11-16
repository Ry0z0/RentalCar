using RentalCar.Entity.Entities;
using RentalCar.Model;
using RentalCar.Repository.Infrastructures;

namespace RentalCar.Repository.Repositories.CarRepositoryRepository
{
    public interface ICarRepository : IBaseRepository<Car>
    {
        public Task<IEnumerable<Guid>> GetAllCarIdOfCarOwner(Guid carOwnerId);
        public Task<IEnumerable<Car>> SearchCarsAsync(string city, string district);
        public Task<IEnumerable<CarStatusDTO>> GetCarStatusesAsync(Guid carOwnerId);

    }
}
