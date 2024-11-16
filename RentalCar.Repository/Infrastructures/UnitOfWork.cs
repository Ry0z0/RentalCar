using Microsoft.EntityFrameworkCore.Storage;
using RentalCar.Entity.Contexts;
using RentalCar.Entity.Entities;
using RentalCar.Repository.Repositories;
using RentalCar.Repository.Repositories.AddressRepository;
using RentalCar.Repository.Repositories.BookingRepository;
using RentalCar.Repository.Repositories.CarOwnerRepository;
using RentalCar.Repository.Repositories.CarRepositoryRepository;
using RentalCar.Repository.Repositories.CustomerRepository;
using RentalCar.Repository.Repositories.FeedbackRepository;

namespace RentalCar.Repository.Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RentalCarDbContext _context;
        private IDbContextTransaction _transaction;
        private bool _disposed;

        public UnitOfWork(RentalCarDbContext context)
        {
            _context = context;
        }

        public RentalCarDbContext Context => _context;

        private ICarRepository _carRepository;
        public ICarRepository CarRepository => _carRepository ??= new CarRepository(_context);

        /*private IQuestionRepository _questionRepository;
        public IQuestionRepository QuestionRepository => _questionRepository ??= new QuestionRepository(_context);

        private IAnswerRepository _answerRepository;
        public IAnswerRepository AnswerRepository => _answerRepository ??= new AnswerRepository(_context);*/

        //private IBaseRepository<Quiz> _quizRepository;
        // public IBaseRepository<Quiz> QuizRepository => _quizRepository ??= new QuizRepository(_context);


        private IBaseRepository<Booking> _bookingRepository;
        public IBaseRepository<Booking> BookingRepository => _bookingRepository ??= new BookingRepository(_context);

        private IBaseRepository<CarOwner> _carOwnerRepository;
        public IBaseRepository<CarOwner> CarOwnerRepository => _carOwnerRepository ??= new CarOwnerRepository(_context);

        private IBaseRepository<Feedback> _feedbackRepository;
        public IBaseRepository<Feedback> FeedbackRepository => _feedbackRepository ??= new FeedbackRepository(_context);

        private IBaseRepository<Customer> _customerRepository;
        public IBaseRepository<Customer> CustomerRepository => _customerRepository ??= new CustomerRepository(_context);

        private AddressCityRepository _addressCityRepository;
        public AddressCityRepository AddressCityRepository => _addressCityRepository ??= new AddressCityRepository(_context);

        private AddressDistrictRepository _addressDistrictRepository;
        public AddressDistrictRepository AddressDistrictRepository => _addressDistrictRepository ??= new AddressDistrictRepository(_context);

        private AddressWardRepository _addressWardRepository;
        public AddressWardRepository AddressWardRepository => _addressWardRepository ??= new AddressWardRepository(_context);
        public IBaseRepository<T> BaseRepository<T>() where T : class
        {
            return new BaseRepository<T>(_context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
