using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalCar.Entity.Entities;
using RentalCar.Repository.Infrastructures;

namespace RentalCar.Repository.Repositories.AdminRepository
{
    public interface IAdminRepository : IBaseRepository<Admin>
    {
    }
}
