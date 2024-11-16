using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
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

public class AddressDistrictService : IAddressDistrictService
{
    private readonly IAddressDistrictRepository _addressDistrictRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<AddressDistrictService> _logger;

    public AddressDistrictService(IUnitOfWork unitOfWork, ILogger<AddressDistrictService> logger, IMapper mapper)
    {
        _addressDistrictRepository = (IAddressDistrictRepository?)unitOfWork.AddressDistrictRepository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<ICollection<AddressDistrict>> GetAllAddressDistrictOfCityAsync(Guid cityId)
    {
        return await _addressDistrictRepository.GetAllAddressDistrictOfCityAsync(cityId);
    }
}

