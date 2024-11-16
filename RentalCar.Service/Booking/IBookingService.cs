using RentalCar.Entity.Entities;
using RentalCar.Model;
using RentalCar.Model;
using RentalCar.Service;
public interface IBookingService : IBaseService<Booking, BookingModel>
{
    public new Task<int> AddAsync(BookingModel bookingModel);
    public Task<int> GetNumberOfBookingOfCarAsync(Guid carId);
    public Task<Booking> GetLastBookingOfTheDayAsync(DateTime date);
    public Task<Booking> GetLatestBookingOfCustomerAsync(Guid customerId);
    public Task<string> GenerateBookingNoAsync(DateTime date);
    public Task<IEnumerable<BookingCarDTO>> GetListBookingCar(Guid CustomerId);

}

