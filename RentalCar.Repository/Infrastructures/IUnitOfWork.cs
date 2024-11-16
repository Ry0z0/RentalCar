using RentalCar.Entity.Contexts;
using RentalCar.Entity.Entities;
using RentalCar.Repository.Repositories.AddressRepository;
using RentalCar.Repository.Repositories.CarRepositoryRepository;

namespace RentalCar.Repository.Infrastructures
{
    public interface IUnitOfWork : IDisposable
    {
        RentalCarDbContext Context { get; }

        /*IQuizRepository QuizRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        IAnswerRepository AnswerRepository { get; }*/

        ICarRepository CarRepository { get; }
        IBaseRepository<Booking> BookingRepository { get; }
        IBaseRepository<CarOwner> CarOwnerRepository { get; }
        IBaseRepository<Feedback> FeedbackRepository { get; }
        IBaseRepository<Customer> CustomerRepository { get; }
        AddressCityRepository AddressCityRepository { get; }
        AddressDistrictRepository AddressDistrictRepository { get; }
        AddressWardRepository AddressWardRepository { get; }

        IBaseRepository<T> BaseRepository<T>() where T : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
