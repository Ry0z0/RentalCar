using Microsoft.EntityFrameworkCore;
using RentalCar.Entity.Contexts;
using RentalCar.Entity.Entities;
using RentalCar.Model;
using RentalCar.Repository.Infrastructures;

namespace RentalCar.Repository.Repositories.CarRepositoryRepository
{
    public class CarRepository : BaseRepository<Car>, ICarRepository
    {
        private readonly RentalCarDbContext _context;

        public CarRepository(RentalCarDbContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<AddressDistrict> GetAllAddressDistrictsOfCity(Guid cityId)
        {
            return _context.AddressDistricts.Where(ad => ad.CityId == cityId).ToList();
        }


        public async Task<IEnumerable<Guid>> GetAllCarIdOfCarOwner(Guid carOwnerId)
        {
            return await _context.Cars
                .Where(c => c.CarOwnerId == carOwnerId)
                .Select(i => i.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> SearchCarsAsync(string city, string district)
        {
            // Tìm kiếm các xe dựa trên địa chỉ chứa city và district
            return await _context.Cars
                .Where(c => c.Active &&
                            (!string.IsNullOrEmpty(city) ? c.Address.Contains(city) : true) &&
                            (!string.IsNullOrEmpty(district) ? c.Address.Contains(district) : true))
                .ToListAsync();
        }
        public async Task<IEnumerable<CarStatusDTO>> GetCarStatusesAsync(Guid carOwnerId)
        {
            var cars = await _context.Cars
                .Where(c => c.CarOwnerId == carOwnerId)
                .ToListAsync();

            var carStatuses = new List<CarStatusDTO>();

            foreach (var car in cars)
            {
                var currentBooking = await _context.Bookings
                    .Where(b => b.CarId == car.Id &&
                                b.StartDateTime <= DateTime.Now &&
                                b.EndDateTime >= DateTime.Now)
                    .OrderByDescending(b => b.StartDateTime)
                    .FirstOrDefaultAsync();

                string status;
                Guid? bookingId = null;

                if (currentBooking != null)
                {
                    status = currentBooking.Status;
                    bookingId = currentBooking.Id; // Lấy bookingId từ booking hiện tại
                }
                else
                {
                    status = "Available";
                }

                carStatuses.Add(new CarStatusDTO
                {
                    CarId = car.Id,
                    CarName = car.Name,
                    Status = status,
                    BookingId = bookingId // Gán giá trị bookingId hoặc null
                });
            }

            return carStatuses;
        }



    }
}
