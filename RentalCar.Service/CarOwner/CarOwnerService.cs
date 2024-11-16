using AutoMapper;
using Microsoft.Extensions.Logging;
using RentalCar.Entity.Entities;
using RentalCar.Model;
using RentalCar.Repository.Infrastructures;
using RentalCar.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalCar.Model;
using RentalCar.Repository.Repositories.CarOwnerRepository;

public class CarOwnerService : BaseService<CarOwner, CarOwnerModel>, ICarOwnerService
    {
        private readonly ICarOwnerRepository _carownerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CarOwnerService> _logger;

        public CarOwnerService(IUnitOfWork unitOfWork, ILogger<CarOwnerService> logger, IMapper mapper)
                : base(unitOfWork, logger, mapper)
        {
            _carownerRepository = unitOfWork.CarOwnerRepository as CarOwnerRepository;
            _mapper = mapper;
            _logger = logger;
        }
    }
