using RentalCar.Entity.Entities;
using RentalCar.Model;
using RentalCar.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public interface IAddressCityService
    {
    public Task<ICollection<AddressCity>> GetAllAddressCityAsync(); 
    }

