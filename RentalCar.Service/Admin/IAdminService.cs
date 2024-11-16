using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalCar.Model;

namespace RentalCar.Service.Admin
{
    public interface IAdminService : IBaseService<Entity.Entities.Admin, AdminModel>
    {
    }
}
