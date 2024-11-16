using AutoMapper;
using Microsoft.Extensions.Logging;
using RentalCar.Entity.Entities;
using RentalCar.Repository.Infrastructures;
using RentalCar.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalCar.Model;
using RentalCar.Repository.Repositories.CustomerRepository;

public class CustomerService : BaseService<Customer, CustomerModel>, ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(IUnitOfWork unitOfWork, ILogger<CustomerService> logger, IMapper mapper)
            : base(unitOfWork, logger, mapper)
    {
        _customerRepository = unitOfWork.CustomerRepository as CustomerRepository;
        _mapper = mapper;
        _logger = logger;
    }
}
