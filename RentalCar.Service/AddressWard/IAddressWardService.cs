using RentalCar.Entity.Entities;
using RentalCar.Model;
using RentalCar.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public interface IAddressWardService
    {
    public Task<ICollection<AddressWard>> GetAllAddressWardOfDistrictAsync(Guid districtId);
    }

