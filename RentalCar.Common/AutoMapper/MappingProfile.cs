using AutoMapper;
using RentalCar.Entity.Entities;
using RentalCar.Model;
using RentalCar.Model;

namespace RentalCar.Common.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Car, CarModel>().ReverseMap();
            CreateMap<CarModel, Car>().ReverseMap();

            CreateMap<Booking, BookingModel>().ReverseMap();
            CreateMap<BookingModel, Booking>().ReverseMap();

            CreateMap<CarOwner, CarOwnerModel>().ReverseMap();
            CreateMap<CarOwnerModel, CarOwner>().ReverseMap();

            CreateMap<Customer, CustomerModel>().ReverseMap();
            CreateMap<CustomerModel, Customer>().ReverseMap();

            CreateMap<Feedback, FeedbackModel>().ReverseMap();
            CreateMap<FeedbackModel, Feedback>().ReverseMap();

            CreateMap<Admin, AdminModel>().ReverseMap();
            CreateMap<AdminModel, Admin>().ReverseMap();

            CreateMap<Feedback, FeedbackCarNameDTOModel>()
                .ForMember(dest => dest.CarName, opt => opt.Ignore());

            CreateMap<Car, SearchCarDTO>()
                .ForMember(dest => dest.NoOfRides, opt => opt.Ignore())
                .ForMember(dest => dest.Ratings, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CarOwnerModel, UserDetailDTO>()
                .ForMember(dest => dest.Role, opt => opt.Ignore()) // Ignore Role when mapping to CustomerModel
                .ReverseMap();

            CreateMap<CustomerModel, UserDetailDTO>()
                .ForMember(dest => dest.Role, opt => opt.Ignore()) // Ignore Role when mapping to CarOwnerModel
                .ReverseMap();



        }
    }
}
