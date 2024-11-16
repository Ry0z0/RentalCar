using RentalCar.Entity.Entities;
using RentalCar.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalCar.Model;

public interface IFeedbackService : IBaseService<Feedback, FeedbackModel>
{
        public Task<int> AddAsync(FeedbackModel dto);
        public Task<double> GetAverageRatingsAsync(Guid carOwnerId);
        public Task<(IEnumerable<FeedbackCarNameDTOModel> Feedbacks, int TotalPages)> GetListFeedbacksAsync(Guid carOwnerId, int index, int feedbackPerPage, int star);
        public Task<IEnumerable<int>> GetNumbersOfRatingAsync(Guid carOwnerId);
        public Task<double> GetAverageRatingsOfCarAsync(Guid carId);

    }
