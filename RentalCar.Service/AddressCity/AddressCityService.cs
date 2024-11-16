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
using RentalCar.Repository.Repositories.AddressRepository;


public class AddressCityService : IAddressCityService
    {
        private readonly IAddressCityRepository _addressCityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressCityService> _logger;

        public AddressCityService(IUnitOfWork unitOfWork, ILogger<AddressCityService> logger, IMapper mapper)
        {
            _addressCityRepository = (IAddressCityRepository?)unitOfWork.AddressCityRepository;
            _mapper = mapper;
            _logger = logger;
        }

    public async Task<ICollection<AddressCity>> GetAllAddressCityAsync()
    {
        return await _addressCityRepository.GetAllAddressCityAsync();
    }
}

