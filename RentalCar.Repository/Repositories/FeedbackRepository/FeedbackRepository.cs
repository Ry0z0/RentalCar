using Microsoft.EntityFrameworkCore;
using RentalCar.Entity.Contexts;
using RentalCar.Entity.Entities;
using RentalCar.Repository.Infrastructures;

namespace RentalCar.Repository.Repositories.FeedbackRepository
{
    public class FeedbackRepository : BaseRepository<Feedback>, IFeedbackRepository
    {
        private readonly RentalCarDbContext _context;

        public FeedbackRepository(RentalCarDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<double> GetAverageRatings(Guid carId)
        {
            var ratings = await _dbSet
                .Where(f => f.Booking.CarId == carId)
                .Select(f => f.Ratings)
                .ToListAsync();

            if (ratings.Any())
            {
                return Math.Round(ratings.Average(), 2);
            }

            return 0; // Return 0 if there are no ratings
        }


        public async Task<IEnumerable<int>> GetNumbersOfRating(Guid carId)
        {
            var ratings = await _dbSet
                .Where(f => f.Booking.CarId == carId)
                .GroupBy(f => f.Ratings)
                .Select(g => new { Rating = g.Key, Count = g.Count() })
                .ToListAsync();

            var ratingCounts = new List<int>(new int[5]);
            foreach (var rating in ratings)
            {
                ratingCounts[rating.Rating - 1] = rating.Count;
            }

            return ratingCounts;
        }

        public async Task<IEnumerable<Feedback>> GetListFeedbacks(Guid carId)
        {
            return await _dbSet.Where(f => f.Booking.CarId == carId).ToListAsync();
        }

        public async Task<double> GetAverageRatingsOfCarAsync(Guid carId)
        {
            var ratings = await _dbSet.Where(f => f.Booking.CarId == carId)
                .Select(f => f.Ratings)
                .ToListAsync();
            return ratings.Any() ? Math.Round(ratings.Average(), 2) : 0;
        }
    }
}
