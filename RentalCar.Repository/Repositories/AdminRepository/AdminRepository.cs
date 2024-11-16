using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalCar.Entity.Contexts;
using RentalCar.Entity.Entities;
using RentalCar.Repository.Infrastructures;

namespace RentalCar.Repository.Repositories.AdminRepository
{
    public class AdminRepository : BaseRepository<Admin>, IAdminRepository
    {
        private readonly RentalCarDbContext _context;

        public AdminRepository(RentalCarDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
