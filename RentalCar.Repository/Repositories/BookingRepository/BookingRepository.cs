using Microsoft.EntityFrameworkCore;
using RentalCar.Entity.Contexts;
using RentalCar.Entity.Entities;
using RentalCar.Repository.Infrastructures;

namespace RentalCar.Repository.Repositories.BookingRepository
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        private readonly RentalCarDbContext _context;

        public BookingRepository(RentalCarDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetNumberOfBookingOfCarAsync(Guid carId)
        {
            return await _context.Bookings.CountAsync(b => b.CarId == carId);
        }

        public async Task<Booking> GetLastBookingForTheDayAsync(DateTime date)
        {
            return await _context.Bookings
                .Where(b => b.StartDateTime.Date >= date.Date)
                .OrderByDescending(b => b.StartDateTime)
                .FirstOrDefaultAsync();
        }

        public async Task<Booking> GetLatestBookingOfCustomerAsync(Guid customerId)
        {
            var booking = await _dbSet.OrderBy(b => b.BookingNo).FirstOrDefaultAsync(b => b.CustomerId == customerId);
            if (booking != null)
            {
                return booking;
            }

            return null;
        }
        public async Task<bool> IsCarBookedAsync(Guid carId, DateTime? startDate, DateTime? endDate)
        {
            // Kiểm tra nếu có booking nào trong khoảng thời gian từ startDate tới endDate không
            if (startDate == null && endDate == null)
            {
                return false; // Nếu cả hai date đều null, nghĩa là không có điều kiện thời gian để kiểm tra.
            }

            return await _context.Bookings
                .AnyAsync(b => b.CarId == carId &&
                               b.Status != "Cancelled" &&
                               (
                                   (startDate != null && endDate != null && b.StartDateTime < endDate && b.EndDateTime > startDate) ||
                                   (startDate != null && endDate == null && b.StartDateTime >= startDate) ||
                                   (startDate == null && endDate != null && b.EndDateTime <= endDate)
                               ));
        }
        public async Task<bool> IsCarCurrentlyRentedAsync(Guid carId)
        {
            return await _context.Bookings
                .AnyAsync(b => b.CarId == carId &&
                               b.Status != "Cancelled" &&
                               b.Status != "Completed" &&
                               b.StartDateTime <= DateTime.Now &&
                               b.EndDateTime >= DateTime.Now);
        }

    }
}
