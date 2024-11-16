using AutoMapper;
using Microsoft.Extensions.Logging;
using RentalCar.Entity.Entities;
using RentalCar.Repository.Infrastructures;
using RentalCar.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentalCar.Repository.Repositories.CarRepositoryRepository;
using RentalCar.Repository.Repositories.FeedbackRepository;
using RentalCar.Model;

public class FeedbackService : BaseService<Feedback, FeedbackModel>, IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ICarRepository _carRepository;
        private readonly ICarService _carService;
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;
        private readonly ILogger<FeedbackService> _logger;

        public FeedbackService(
            IUnitOfWork unitOfWork,
            ILogger<FeedbackService> logger,
            IMapper mapper,
            ICarService carService,
            IBookingService bookingService)  // Inject CarService via constructor
            : base(unitOfWork, logger, mapper)
        {
            _feedbackRepository = unitOfWork.FeedbackRepository as FeedbackRepository;
            _carRepository = unitOfWork.CarRepository as CarRepository;
            _mapper = mapper;
            _logger = logger;
            _carService = carService;  // Assign CarService to a private field
            _bookingService = bookingService;
        }

        public override async Task<int> AddAsync(FeedbackModel dto)
        {
            _logger.LogInformation("Adding a new feedback entity.");
            try
            {
                // Ignore the incoming Id and generate a new one
                dto.Id = Guid.NewGuid();

                // Map the DTO to the entity
                var entity = _mapper.Map<Feedback>(dto);

                // Add the feedback entity
                _repository.Add(entity);
                var result = await _unitOfWork.SaveChangesAsync();

                var booking = _bookingService.GetByIdAsync(entity.BookingId).Result;

                booking.FeedbackId = entity.Id;
                await _bookingService.UpdateAsync(booking);
                _logger.LogInformation("Feedback entity added successfully.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding feedback entity.");
                throw;
            }
        }

        public async Task<double> GetAverageRatingsAsync(Guid carOwnerId)
        {
            double avg = 0;
            double total = 0;
            int index = 0;

            IEnumerable<Guid> carIds = await _carRepository.GetAllCarIdOfCarOwner(carOwnerId);

            foreach (Guid carId in carIds)
            {
                var carAvgRating = await _feedbackRepository.GetAverageRatings(carId);

                if (carAvgRating > 0) // Only consider cars that have ratings
                {
                    index++;
                    total += carAvgRating;
                }
            }

            if (index > 0)
                avg = total / index;

            return Math.Round(avg, 2);
        }


    public async Task<IEnumerable<int>> AggregateRatings(IEnumerable<IEnumerable<int>> listOfRatings)
    {
        var aggregatedRatings = new int[5];

        foreach (var ratings in listOfRatings)
        {
            for (int i = 0; i < ratings.Count(); i++)
            {
                aggregatedRatings[i] += ratings.ElementAt(i);
            }
        }

        return aggregatedRatings;
    }


    public async Task<IEnumerable<int>> GetNumbersOfRatingAsync(Guid carOwnerId)
    {
        IEnumerable<Guid> carIds = await _carRepository.GetAllCarIdOfCarOwner(carOwnerId);
        var listOfRatings = new List<IEnumerable<int>>();
        foreach (Guid carId in carIds)
        {
            var ratings = await _feedbackRepository.GetNumbersOfRating(carId);
            listOfRatings.Add(ratings);
        }
        if (!listOfRatings.Any())
        {
            return new int[5];
        }
        var aggregatedRatings = await AggregateRatings(listOfRatings);

        return aggregatedRatings;
    }


    public async Task<(IEnumerable<FeedbackCarNameDTOModel> Feedbacks, int TotalPages)> GetListFeedbacksAsync(Guid carOwnerId, int star, int index, int feedbackPerPage)
    {
        try
        {
            // Get all car IDs for the car owner
            var carIds = await _carRepository.GetAllCarIdOfCarOwner(carOwnerId);

            // Filter the feedbacks based on car IDs and the star rating
            var query = _feedbackRepository
                .GetQuery() // Assuming GetQuery is a method in BaseRepository to get the queryable set
                .Where(f => carIds.Contains(f.Booking.CarId) && (star == 0 || f.Ratings == star));

            // Get total feedback count for pagination calculation
            var totalFeedbackCount = await query.CountAsync();

            // Calculate total pages
            int totalPages = (int)Math.Ceiling(totalFeedbackCount / (double)feedbackPerPage);

            // Apply pagination and retrieve feedbacks
            var feedbacks = await query
                .OrderByDescending(f => f.DateTime) // Order by latest feedback
                .Skip((index - 1) * feedbackPerPage)
                .Take(feedbackPerPage)
                .ToListAsync();

            // Map each FeedbackModel to FeedbackCarNameDTO including car name
            var feedbackDTOs = new List<FeedbackCarNameDTOModel>();

            foreach (var feedback in feedbacks)
            {
                var carName = await _carService.GetCarNameInBooking(feedback.BookingId);
                var feedbackDTO = _mapper.Map<FeedbackCarNameDTOModel>(feedback);
                feedbackDTO.CarName = carName; // Add car name to DTO
                feedbackDTOs.Add(feedbackDTO);
            }

            return (feedbackDTOs, totalPages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching feedback list.");
            return (Enumerable.Empty<FeedbackCarNameDTOModel>(), 0);
        }
    }
    public async Task<double> GetAverageRatingsOfCarAsync(Guid carId)
    {
        try
        {
            return await _feedbackRepository.GetAverageRatingsOfCarAsync(carId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting average ratings for car {carId}");
            throw;
        }
    }

}
