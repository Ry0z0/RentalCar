using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RentalCar.Entity.Contexts;
using RentalCar.Model;
using RentalCar.Repository.Infrastructures;
using RentalCar.Repository.Repositories.AdminRepository;
using RentalCar.Repository.Repositories.BookingRepository;

namespace RentalCar.Service.Admin
{
    public class AdminService : BaseService<Entity.Entities.Admin, AdminModel>, IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminService> _logger;

        public AdminService(IUnitOfWork unitOfWork, ILogger<AdminService> logger, IMapper mapper)
            : base(unitOfWork, logger, mapper)
        {
            _adminRepository = unitOfWork.BookingRepository as AdminRepository;
            _mapper = mapper;
            _logger = logger;
        }
    }
}
