using AutoMapper;
using Microsoft.Extensions.Logging;
using RentalCar.Entity.Entities;
using RentalCar.Model;
using RentalCar.Model;
using RentalCar.Repository.Infrastructures;
using RentalCar.Repository.Repositories.BookingRepository;
using RentalCar.Repository.Repositories.CarRepositoryRepository;
using RentalCar.Service;

public class BookingService : BaseService<Booking, BookingModel>, IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<BookingService> _logger;
    private readonly ICarRepository _carRepository;
    private static readonly object _bookingNoLock = new object();

    public BookingService(IUnitOfWork unitOfWork, ILogger<BookingService> logger, IMapper mapper)
        : base(unitOfWork, logger, mapper)
    {
        _bookingRepository = unitOfWork.BookingRepository as BookingRepository;
        _mapper = mapper;
        _logger = logger;
        _carRepository = unitOfWork.CarRepository as CarRepository;
    }

    public override async Task<int> AddAsync(BookingModel bookingModel)
    {
        _logger.LogInformation("Adding a new booking.");
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var booking = _mapper.Map<Booking>(bookingModel);
            booking.Id = Guid.NewGuid();
            booking.BookingNo = await GenerateBookingNoAsync(booking.StartDateTime.Date);
            _bookingRepository.Add(booking);
            var result = await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation("Booking added successfully.");
            return result;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Error adding booking.");
            throw;
        }
    }

    public async Task<string> GenerateBookingNoAsync(DateTime date)
    {
        int increment = 1;

        lock (_bookingNoLock)
        {
            var lastBooking = _bookingRepository.GetLastBookingForTheDayAsync(date).Result;

            if (lastBooking != null)
            {
                var lastIncrement = int.Parse(lastBooking.BookingNo.Substring(9));
                increment = lastIncrement + 1;
            }
        }

        return $"{date.Date:yyyyMMdd}-{increment:D4}"; // Format: yyyyMMdd-0001, yyyyMMdd-0002, etc.
    }

    public async Task<Booking> GetLastBookingOfTheDayAsync(DateTime date)
    {
        return await _bookingRepository.GetLastBookingForTheDayAsync(date);
    }

    public async Task<int> GetNumberOfBookingOfCarAsync(Guid carId)
    {
        try
        {
            return await _bookingRepository.GetNumberOfBookingOfCarAsync(carId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting number of booking of {carId}");
            throw;
        }
    }

    public async Task<Booking> GetLatestBookingOfCustomerAsync(Guid customerId)
    {
        try
        {
            return await _bookingRepository.GetLatestBookingOfCustomerAsync(customerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting latest booking of customer: {customerId}");
            throw;
        }
    }

    public async Task<IEnumerable<BookingCarDTO>> GetListBookingCar(Guid CustomerId)
    {
        IEnumerable<Booking> bookings = _bookingRepository.GetQuery().Where(c => c.CustomerId == CustomerId).ToList();
        List<BookingCarDTO>
            bookingCarDTOs =
                new List<BookingCarDTO>(); // Sử dụng List<BookingCarDTO> thay vì IEnumerable<BookingCarDTO>

        foreach (Booking booking in bookings)
        {
            var car = _carRepository.GetById(booking.CarId);
            var bookingCar = new BookingCarDTO
            {
                BookingNo = booking.BookingNo,
                CarId = booking.CarId,
                BookingId = booking.Id, // Sửa thành booking.Id thay vì booking.CarId
                CarName = car.Name,
                StartDateTime = booking.StartDateTime,
                EndDateTime = booking.EndDateTime,
                BasePrice = car.BasePrice,
                Deposit = car.Deposit,
                Status = booking.Status
            };
            bookingCarDTOs.Add(bookingCar); // Sử dụng Add để thêm phần tử vào danh sách
        }

        return bookingCarDTOs;
    }

}

