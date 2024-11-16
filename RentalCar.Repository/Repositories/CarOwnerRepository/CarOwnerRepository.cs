using RentalCar.Entity.Contexts;
using RentalCar.Entity.Entities;
using RentalCar.Repository.Infrastructures;

namespace RentalCar.Repository.Repositories.CarOwnerRepository;

public class CarOwnerRepository : BaseRepository<CarOwner>, ICarOwnerRepository
{
    private readonly RentalCarDbContext _context;

    public CarOwnerRepository(RentalCarDbContext context) : base(context)
    {
        _context = context;
    }

    public string GetCarStatus(DateTime date)
    {
        return "active";
    }

    public CarOwner? GetUserByEmail(string email)
    {
        var user = _context.CarOwners.FirstOrDefault(e => e.Email == email);
        return user;
    }

    public bool CheckPasswordHash(CarOwner carOwner, string password)
    {
        var passwordHash = HashPassword(password);
        return carOwner.PasswordHash == passwordHash;
    }
}