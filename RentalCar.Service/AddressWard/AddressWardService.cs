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


public class AddressWardService : IAddressWardService
    {
        private readonly IAddressWardRepository _addressWardRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressWardService> _logger;

    public AddressWardService(IUnitOfWork unitOfWork, ILogger<AddressWardService> logger, IMapper mapper)
    {
            _addressWardRepository = (IAddressWardRepository?)unitOfWork.AddressWardRepository;
            _mapper = mapper;
            _logger = logger;
    }

    public async Task<ICollection<AddressWard>> GetAllAddressWardOfDistrictAsync(Guid districtId)
    {
        return await _addressWardRepository.GetAllAddressWardOfDistrictAsync(districtId);
    }
}

