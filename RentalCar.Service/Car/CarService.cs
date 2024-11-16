using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RentalCar.Entity.Entities;
using RentalCar.Model;
using RentalCar.Repository.Infrastructures;
using RentalCar.Repository.Repositories.BookingRepository;
using RentalCar.Repository.Repositories.CarRepositoryRepository;
using RentalCar.Repository.Repositories.FeedbackRepository;
using RentalCar.Service;


public class CarService : BaseService<Car, CarModel>, ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly IBookingRepository _bookRepository;
    private readonly IFeedbackRepository _feedbackRepository;

    private readonly IMapper _mapper;
    private readonly ILogger<CarService> _logger;

    public CarService(IUnitOfWork unitOfWork, ILogger<CarService> logger, IMapper mapper)
            : base(unitOfWork, logger, mapper)
    {
        _carRepository = unitOfWork.CarRepository as CarRepository;
        _bookRepository = unitOfWork.BookingRepository as IBookingRepository;
        _feedbackRepository = unitOfWork.FeedbackRepository as IFeedbackRepository;
        ;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> GetCarNameInBooking(Guid bookingId)
    {
        var booking = _bookRepository.GetById(bookingId);
        var car = await _carRepository.GetQuery().FirstOrDefaultAsync(c => c.Id == booking.CarId);

        return car.Name;
    }

    public async Task<IEnumerable<SearchCarDTO>> SearchCarAsync(DateTime? startDate, DateTime? endDate, string location)
    {
        // Validate the input: if startDate >= endDate, throw an error
        if (startDate != null && endDate != null && startDate >= endDate)
        {
            throw new ArgumentException("Start date must be less than end date.");
        }

        string city = null;
        string district = null;

        if (!string.IsNullOrEmpty(location))
        {
            var locationParts = location.Split(',');
            city = locationParts.Length > 0 ? locationParts[0] : null;
            district = locationParts.Length > 1 ? locationParts[1] : null;
        }

        // Find cars by city and district (if provided)
        var cars = await _carRepository.SearchCarsAsync(city, district);

        // If startDate and endDate are not provided, return all cars found
        if (startDate == null && endDate == null)
        {
            return await MapToSearchCarDTO(cars);
        }

        // Filter out cars that are booked during the given time frame
        var availableCars = new List<Car>();

        foreach (var car in cars)
        {
            var isBooked = await _bookRepository.IsCarBookedAsync(car.Id, (DateTime)startDate, (DateTime)endDate);
            if (!isBooked)
            {
                availableCars.Add(car);
            }
        }

        return await MapToSearchCarDTO(availableCars);
    }

    private async Task<IEnumerable<SearchCarDTO>> MapToSearchCarDTO(IEnumerable<Car> cars)
    {
        var carDtos = new List<SearchCarDTO>();

        foreach (var car in cars)
        {
            var noOfRides = await _bookRepository.GetNumberOfBookingOfCarAsync(car.Id);
            var ratings = await _feedbackRepository.GetAverageRatingsOfCarAsync(car.Id);

            var carDto = _mapper.Map<SearchCarDTO>(car);
            carDto.NoOfRides = noOfRides;
            carDto.Ratings = ratings;

            carDtos.Add(carDto);
        }

        return carDtos;
    }

    public async Task<IEnumerable<SearchCarDTO>> SearchCarByOwnerAsync(Guid carOwnerId)
    {
        // Lấy tất cả các xe thuộc về carOwnerId
        var cars = await _carRepository.GetQuery(c => c.CarOwnerId == carOwnerId).ToListAsync();

        // Map các xe này sang DTO bao gồm cả số lượng rides và ratings
        return await MapToSearchCarDTO(cars);
    }

    public async Task<IEnumerable<CarStatusDTO>> GetCarStatusesForOwnerAsync(Guid carOwnerId)
    {
        return await _carRepository.GetCarStatusesAsync(carOwnerId);
    }

    public async Task<IEnumerable<SearchCarDTO>> GetAllCarsForAdminAsync()
    {
        // Get all cars from the repository
        var cars = await _carRepository.GetAllAsync();

        // Map the cars to the SearchCarDTO, including the number of rides and ratings
        return await MapToSearchCarDTO(cars);
    }

    public async Task<CarModel> UpdateCarAsync(CarModel carModel)
    {
        var existingCar = await _carRepository.GetByIdAsync(carModel.Id);

        if (existingCar == null)
        {
            return null;  // Car not found
        }

        _mapper.Map(carModel, existingCar);  // Map updated details onto existing car entity

        _carRepository.Update(existingCar);

        return _mapper.Map<CarModel>(existingCar);  // Return the updated car as a model
    }
    public override async Task<bool> UpdateAsync(CarModel carModel)
    {
        var existingCar = await _carRepository.GetByIdAsync(carModel.Id);

        if (existingCar == null)
        {
            return false;  // Car not found
        }

        _mapper.Map(carModel, existingCar);  // Map updated details onto existing car entity

        _carRepository.Detach(existingCar);
        _carRepository.Update(existingCar);
        await _unitOfWork.SaveChangesAsync();
        return true;  // Return the updated car as a model
    }
}

