using RentalCar.Entity.Entities;
using RentalCar.Model;
using RentalCar.Repository.Infrastructures;

namespace RentalCar.Repository.Repositories.FeedbackRepository
{
    public interface IFeedbackRepository : IBaseRepository<Feedback>
    {
        Task<double> GetAverageRatings(Guid carId);
        Task<IEnumerable<int>> GetNumbersOfRating(Guid carId);
        Task<IEnumerable<Feedback>> GetListFeedbacks(Guid carId);

        Task<double> GetAverageRatingsOfCarAsync(Guid carId);
    }
}
