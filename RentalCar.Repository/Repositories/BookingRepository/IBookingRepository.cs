using RentalCar.Entity.Entities;
using RentalCar.Repository.Infrastructures;

namespace RentalCar.Repository.Repositories.BookingRepository
{
    public interface IBookingRepository : IBaseRepository<Booking>
    {
        public Task<int> GetNumberOfBookingOfCarAsync(Guid carId);
        public Task<Booking> GetLastBookingForTheDayAsync(DateTime date);
        public Task<Booking> GetLatestBookingOfCustomerAsync(Guid customerId);
        public Task<bool> IsCarBookedAsync(Guid carId, DateTime? startDate, DateTime? endDate);
        public Task<bool> IsCarCurrentlyRentedAsync(Guid carId);

    }
}
