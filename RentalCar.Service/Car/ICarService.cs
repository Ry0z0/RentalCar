using RentalCar.Entity.Entities;
using RentalCar.Model;
using RentalCar.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalCar.Model;

public interface ICarService : IBaseService<Car, CarModel>
    {
        public Task<string> GetCarNameInBooking(Guid bookingId);
        public Task<IEnumerable<SearchCarDTO>> SearchCarAsync(DateTime? startDate, DateTime? endDate, string location);
        public Task<IEnumerable<SearchCarDTO>> SearchCarByOwnerAsync(Guid carOwnerId);
        public Task<IEnumerable<CarStatusDTO>> GetCarStatusesForOwnerAsync(Guid carOwnerId);
        public Task<IEnumerable<SearchCarDTO>> GetAllCarsForAdminAsync();
        public Task<CarModel> UpdateCarAsync(CarModel carModel);
        public new Task<bool> UpdateAsync(CarModel carModel);
    }

