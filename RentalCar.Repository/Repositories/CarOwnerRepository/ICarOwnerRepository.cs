using RentalCar.Entity.Entities;
using RentalCar.Repository.Infrastructures;

namespace RentalCar.Repository.Repositories.CarOwnerRepository;

public interface ICarOwnerRepository : IBaseRepository<CarOwner>
{
    public CarOwner? GetUserByEmail(string email);
    public bool CheckPasswordHash(CarOwner carOwner, string password);
}