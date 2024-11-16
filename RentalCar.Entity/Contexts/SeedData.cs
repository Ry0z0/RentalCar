using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RentalCar.Entity.Contexts;
using RentalCar.Entity.Entities;
using ExcelDataReader;

namespace RentalCar.Entity
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RentalCarDbContext(
                       serviceProvider.GetRequiredService<DbContextOptions<RentalCarDbContext>>()))
            {
                // Ensure the database is created
                await context.Database.EnsureCreatedAsync();

                // Seed CarOwners
                if (!context.CarOwners.Any())
                {
                    context.CarOwners.AddRange(
                        new CarOwner
                        {
                            Id = Guid.NewGuid(),
                            Name = "CarOwner One",
                            DateOfBirth = new DateTime(1980, 1, 1),
                            NationalIdNo = "1234567890",
                            PhoneNo = "123456789",
                            Email = "carowner1@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL12345"
                        },
                        new CarOwner
                        {
                            Id = Guid.NewGuid(),
                            Name = "CarOwner Two",
                            DateOfBirth = new DateTime(1982, 2, 2),
                            NationalIdNo = "0987654321",
                            PhoneNo = "987654321",
                            Email = "carowner2@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL67890"
                        },
                        // Add more CarOwners here...
                        new CarOwner
                        {
                            Id = Guid.NewGuid(),
                            Name = "CarOwner Three",
                            DateOfBirth = new DateTime(1984, 3, 3),
                            NationalIdNo = "9876543210",
                            PhoneNo = "987654321",
                            Email = "carowner3@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL54321"
                        },
                        new CarOwner
                        {
                            Id = Guid.NewGuid(),
                            Name = "CarOwner Four",
                            DateOfBirth = new DateTime(1986, 4, 4),
                            NationalIdNo = "8765432109",
                            PhoneNo = "876543210",
                            Email = "carowner4@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL43210"
                        },
                        new CarOwner
                        {
                            Id = Guid.NewGuid(),
                            Name = "CarOwner Five",
                            DateOfBirth = new DateTime(1988, 5, 5),
                            NationalIdNo = "7654321098",
                            PhoneNo = "765432109",
                            Email = "carowner5@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL32109"
                        },
                        new CarOwner
                        {
                            Id = Guid.NewGuid(),
                            Name = "CarOwner Six",
                            DateOfBirth = new DateTime(1990, 6, 6),
                            NationalIdNo = "6543210987",
                            PhoneNo = "654321098",
                            Email = "carowner6@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL21098"
                        },
                        new CarOwner
                        {
                            Id = Guid.NewGuid(),
                            Name = "CarOwner Seven",
                            DateOfBirth = new DateTime(1992, 7, 7),
                            NationalIdNo = "5432109876",
                            PhoneNo = "543210987",
                            Email = "carowner7@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL10987"
                        },
                        new CarOwner
                        {
                            Id = Guid.NewGuid(),
                            Name = "CarOwner Eight",
                            DateOfBirth = new DateTime(1994, 8, 8),
                            NationalIdNo = "4321098765",
                            PhoneNo = "432109876",
                            Email = "carowner8@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL09876"
                        },
                        new CarOwner
                        {
                            Id = Guid.NewGuid(),
                            Name = "CarOwner Nine",
                            DateOfBirth = new DateTime(1996, 9, 9),
                            NationalIdNo = "3210987654",
                            PhoneNo = "321098765",
                            Email = "carowner9@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL98765"
                        },
                        new CarOwner
                        {
                            Id = Guid.NewGuid(),
                            Name = "CarOwner Ten",
                            DateOfBirth = new DateTime(1998, 10, 10),
                            NationalIdNo = "2109876543",
                            PhoneNo = "210987654",
                            Email = "carowner10@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL87654"
                        }
                    );
                    await context.SaveChangesAsync();
                }

                // Seed Customers
                if (!context.Customers.Any())
                {
                    context.Customers.AddRange(
                        new Customer
                        {
                            Id = Guid.NewGuid(),
                            Name = "Customer One",
                            DateOfBirth = new DateTime(1990, 1, 1),
                            NationalIdNo = "1111111111",
                            PhoneNo = "111111111",
                            Email = "customer1@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL11111"
                        },
                        new Customer
                        {
                            Id = Guid.NewGuid(),
                            Name = "Customer Two",
                            DateOfBirth = new DateTime(1992, 2, 2),
                            NationalIdNo = "2222222222",
                            PhoneNo = "222222222",
                            Email = "customer2@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL22222"
                        },
                        // Add more Customers here...
                        new Customer
                        {
                            Id = Guid.NewGuid(),
                            Name = "Customer Three",
                            DateOfBirth = new DateTime(1994, 3, 3),
                            NationalIdNo = "3333333333",
                            PhoneNo = "333333333",
                            Email = "customer3@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL33333"
                        },
                        new Customer
                        {
                            Id = Guid.NewGuid(),
                            Name = "Customer Four",
                            DateOfBirth = new DateTime(1996, 4, 4),
                            NationalIdNo = "4444444444",
                            PhoneNo = "444444444",
                            Email = "customer4@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL44444"
                        },
                        new Customer
                        {
                            Id = Guid.NewGuid(),
                            Name = "Customer Five",
                            DateOfBirth = new DateTime(1998, 5, 5),
                            NationalIdNo = "5555555555",
                            PhoneNo = "555555555",
                            Email = "customer5@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL55555"
                        },
                        new Customer
                        {
                            Id = Guid.NewGuid(),
                            Name = "Customer Six",
                            DateOfBirth = new DateTime(2000, 6, 6),
                            NationalIdNo = "6666666666",
                            PhoneNo = "666666666",
                            Email = "customer6@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL66666"
                        },
                        new Customer
                        {
                            Id = Guid.NewGuid(),
                            Name = "Customer Seven",
                            DateOfBirth = new DateTime(2002, 7, 7),
                            NationalIdNo = "7777777777",
                            PhoneNo = "777777777",
                            Email = "customer7@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL77777"
                        },
                        new Customer
                        {
                            Id = Guid.NewGuid(),
                            Name = "Customer Eight",
                            DateOfBirth = new DateTime(2004, 8, 8),
                            NationalIdNo = "8888888888",
                            PhoneNo = "888888888",
                            Email = "customer8@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL88888"
                        },
                        new Customer
                        {
                            Id = Guid.NewGuid(),
                            Name = "Customer Nine",
                            DateOfBirth = new DateTime(2006, 9, 9),
                            NationalIdNo = "9999999999",
                            PhoneNo = "999999999",
                            Email = "customer9@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL99999"
                        },
                        new Customer
                        {
                            Id = Guid.NewGuid(),
                            Name = "Customer Ten",
                            DateOfBirth = new DateTime(2008, 10, 10),
                            NationalIdNo = "1010101010",
                            PhoneNo = "101010101",
                            Email = "customer10@example.com",
                            PasswordHash = HashPassword("Password123!"),
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            DrivingLicense = "DL10101"
                        }
                    );
                    await context.SaveChangesAsync();
                }

                // Seed Cars
                if (!context.Cars.Any())
                {
                    var carOwners = context.CarOwners.Take(10).ToList();

                    context.Cars.AddRange(
                        new Car
                        {
                            Id = Guid.NewGuid(),
                            Name = "Toyota Camry",
                            LicensePlate = "ABC123",
                            Brand = "Toyota",
                            Model = "Camry",
                            Color = "Black",
                            NumberOfSeats = 5,
                            ProductionYears = 2020,
                            TransmissionType = "Automatic",
                            FuelType = "Gasoline",
                            Mileage = 10000,
                            FuelConsumption = 8.5,
                            BasePrice = 30000,
                            Images = "img/toyota.jpg",
                            Deposit = 500,
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            CarOwnerId = carOwners[0].Id
                        },
                        new Car
                        {
                            Id = Guid.NewGuid(),
                            Name = "Honda Civic",
                            LicensePlate = "XYZ789",
                            Brand = "Honda",
                            Model = "Civic",
                            Color = "Blue",
                            NumberOfSeats = 4,
                            ProductionYears = 2019,
                            TransmissionType = "Manual",
                            FuelType = "Diesel",
                            Mileage = 15000,
                            FuelConsumption = 7.0,
                            BasePrice = 25000,
                            Deposit = 400,
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            Images = "img/honda.jpg",
                            CarOwnerId = carOwners[1].Id
                        },
                        // Add more Cars here...
                        new Car
                        {
                            Id = Guid.NewGuid(),
                            Name = "Ford Focus",
                            LicensePlate = "FOC123",
                            Brand = "Ford",
                            Model = "Focus",
                            Color = "White",
                            NumberOfSeats = 5,
                            ProductionYears = 2018,
                            TransmissionType = "Automatic",
                            FuelType = "Gasoline",
                            Mileage = 20000,
                            FuelConsumption = 9.0,
                            BasePrice = 22000,
                            Deposit = 600,
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            Images = "img/ford.jpg",
                            CarOwnerId = carOwners[2].Id
                        },
                        new Car
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chevrolet Malibu",
                            LicensePlate = "CHEV123",
                            Brand = "Chevrolet",
                            Model = "Malibu",
                            Color = "Red",
                            NumberOfSeats = 5,
                            ProductionYears = 2017,
                            TransmissionType = "Automatic",
                            FuelType = "Gasoline",
                            Mileage = 25000,
                            FuelConsumption = 8.0,
                            BasePrice = 28000,
                            Deposit = 700,
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            Images = "img/chevrolet2.jpg",
                            CarOwnerId = carOwners[3].Id
                        },
                        new Car
                        {
                            Id = Guid.NewGuid(),
                            Name = "Nissan Altima",
                            LicensePlate = "NIS123",
                            Brand = "Nissan",
                            Model = "Altima",
                            Color = "Silver",
                            NumberOfSeats = 5,
                            ProductionYears = 2016,
                            TransmissionType = "Manual",
                            FuelType = "Diesel",
                            Mileage = 30000,
                            FuelConsumption = 7.5,
                            BasePrice = 27000,
                            Deposit = 800,
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            Images = "img/nissan.jpg",
                            CarOwnerId = carOwners[4].Id
                        },
                        new Car
                        {
                            Id = Guid.NewGuid(),
                            Name = "BMW 3 Series",
                            LicensePlate = "BMW123",
                            Brand = "BMW",
                            Model = "3 Series",
                            Color = "Black",
                            NumberOfSeats = 5,
                            ProductionYears = 2015,
                            TransmissionType = "Automatic",
                            FuelType = "Gasoline",
                            Mileage = 35000,
                            FuelConsumption = 9.5,
                            BasePrice = 45000,
                            Deposit = 1000,
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            Images = "img/bmw.jpg",
                            CarOwnerId = carOwners[5].Id
                        },
                        new Car
                        {
                            Id = Guid.NewGuid(),
                            Name = "Audi A4",
                            LicensePlate = "AUDI123",
                            Brand = "Audi",
                            Model = "A4",
                            Color = "Blue",
                            NumberOfSeats = 5,
                            ProductionYears = 2014,
                            TransmissionType = "Automatic",
                            FuelType = "Gasoline",
                            Mileage = 40000,
                            FuelConsumption = 10.0,
                            BasePrice = 50000,
                            Deposit = 1200,
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            Images = "img/audi.jpg",
                            CarOwnerId = carOwners[6].Id
                        },
                        new Car
                        {
                            Id = Guid.NewGuid(),
                            Name = "Mercedes-Benz C-Class",
                            LicensePlate = "MERC123",
                            Brand = "Mercedes-Benz",
                            Model = "C-Class",
                            Color = "White",
                            NumberOfSeats = 5,
                            ProductionYears = 2013,
                            TransmissionType = "Automatic",
                            FuelType = "Gasoline",
                            Mileage = 45000,
                            FuelConsumption = 11.0,
                            BasePrice = 55000,
                            Deposit = 1400,
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            Images = "img/mec3.jpg",
                            CarOwnerId = carOwners[7].Id
                        },
                        new Car
                        {
                            Id = Guid.NewGuid(),
                            Name = "Volkswagen Passat",
                            LicensePlate = "VW123",
                            Brand = "Volkswagen",
                            Model = "Passat",
                            Color = "Black",
                            NumberOfSeats = 5,
                            ProductionYears = 2012,
                            TransmissionType = "Manual",
                            FuelType = "Diesel",
                            Mileage = 50000,
                            FuelConsumption = 6.5,
                            BasePrice = 24000,
                            Deposit = 1100,
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            Images = "img/volkswagen.jpg",
                            CarOwnerId = carOwners[8].Id
                        },
                        new Car
                        {
                            Id = Guid.NewGuid(),
                            Name = "Hyundai Sonata",
                            LicensePlate = "HYUN123",
                            Brand = "Hyundai",
                            Model = "Sonata",
                            Color = "Gray",
                            NumberOfSeats = 5,
                            ProductionYears = 2011,
                            TransmissionType = "Automatic",
                            FuelType = "Gasoline",
                            Mileage = 55000,
                            FuelConsumption = 7.8,
                            BasePrice = 21000,
                            Deposit = 900,
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            Images = "img/huyndai.jpg",
                            CarOwnerId = carOwners[9].Id
                        }
                    );
                    await context.SaveChangesAsync();
                }

                // Seed Bookings
                if (!context.Bookings.Any())
                {
                    var customers = context.Customers.Take(10).ToList();
                    var cars = context.Cars.Take(10).ToList();

                    context.Bookings.AddRange(
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "08072024-1",
                            StartDateTime = DateTime.Now,
                            EndDateTime = DateTime.Now.AddDays(5),
                            DriversInformation = "Driver 1",
                            PaymentMethod = "Credit Card",
                            Status = "Confirmed",
                            CustomerId = customers[0].Id,
                            CarId = cars[0].Id,
                            FeedbackId = null 
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "08072024-2",
                            StartDateTime = DateTime.Now.AddDays(10),
                            EndDateTime = DateTime.Now.AddDays(15),
                            DriversInformation = "Driver 2",
                            PaymentMethod = "PayPal",
                            Status = "Completed",
                            CustomerId = customers[1].Id,
                            CarId = cars[1].Id,
                            FeedbackId = null // This needs to be replaced with actual feedback seeding
                        },
                        // Add more Bookings here...
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "08072024-3",
                            StartDateTime = DateTime.Now.AddDays(20),
                            EndDateTime = DateTime.Now.AddDays(25),
                            DriversInformation = "Driver 3",
                            PaymentMethod = "Debit Card",
                            Status = "Confirmed",
                            CustomerId = customers[2].Id,
                            CarId = cars[2].Id,
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "08072024-4",
                            StartDateTime = DateTime.Now.AddDays(30),
                            EndDateTime = DateTime.Now.AddDays(35),
                            DriversInformation = "Driver 4",
                            PaymentMethod = "Cash",
                            Status = "Cancelled",
                            CustomerId = customers[3].Id,
                            CarId = cars[3].Id,
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "08072024-5",
                            StartDateTime = DateTime.Now.AddDays(40),
                            EndDateTime = DateTime.Now.AddDays(45),
                            DriversInformation = "Driver 5",
                            PaymentMethod = "Credit Card",
                            Status = "Confirmed",
                            CustomerId = customers[4].Id,
                            CarId = cars[4].Id,
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "08072024-6",
                            StartDateTime = DateTime.Now.AddDays(50),
                            EndDateTime = DateTime.Now.AddDays(55),
                            DriversInformation = "Driver 6",
                            PaymentMethod = "PayPal",
                            Status = "Completed",
                            CustomerId = customers[5].Id,
                            CarId = cars[5].Id,
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "08072024-7",
                            StartDateTime = DateTime.Now.AddDays(60),
                            EndDateTime = DateTime.Now.AddDays(65),
                            DriversInformation = "Driver 7",
                            PaymentMethod = "Debit Card",
                            Status = "Confirmed",
                            CustomerId = customers[6].Id,
                            CarId = cars[6].Id,
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "08072024-8",
                            StartDateTime = DateTime.Now.AddDays(70),
                            EndDateTime = DateTime.Now.AddDays(75),
                            DriversInformation = "Driver 8",
                            PaymentMethod = "Cash",
                            Status = "Cancelled",
                            CustomerId = customers[7].Id,
                            CarId = cars[7].Id,
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "08072024-9",
                            StartDateTime = DateTime.Now.AddDays(80),
                            EndDateTime = DateTime.Now.AddDays(85),
                            DriversInformation = "Driver 9",
                            PaymentMethod = "Credit Card",
                            Status = "Confirmed",
                            CustomerId = customers[8].Id,
                            CarId = cars[8].Id,
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "08072024-10",
                            StartDateTime = DateTime.Now.AddDays(90),
                            EndDateTime = DateTime.Now.AddDays(95),
                            DriversInformation = "Driver 10",
                            PaymentMethod = "PayPal",
                            Status = "Completed",
                            CustomerId = customers[9].Id,
                            CarId = cars[9].Id,
                            FeedbackId = null
                        }
                    );
                    await context.SaveChangesAsync();
                }

                // Seed Feedback
                if (!context.Feedbacks.Any())
                {
                    var bookings = context.Bookings.Take(10).ToList();

                    context.Feedbacks.AddRange(
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 5,
                            Content = "Great service!",
                            DateTime = DateTime.Now,
                            BookingId = bookings[0].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 4,
                            Content = "Very good experience.",
                            DateTime = DateTime.Now.AddDays(1),
                            BookingId = bookings[1].Id
                        },
                        // Add more Feedback here...
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 3,
                            Content = "It was okay.",
                            DateTime = DateTime.Now.AddDays(2),
                            BookingId = bookings[2].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 2,
                            Content = "Not very satisfied.",
                            DateTime = DateTime.Now.AddDays(3),
                            BookingId = bookings[3].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 1,
                            Content = "Terrible experience!",
                            DateTime = DateTime.Now.AddDays(4),
                            BookingId = bookings[4].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 5,
                            Content = "Excellent service!",
                            DateTime = DateTime.Now.AddDays(5),
                            BookingId = bookings[5].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 4,
                            Content = "Good overall.",
                            DateTime = DateTime.Now.AddDays(6),
                            BookingId = bookings[6].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 3,
                            Content = "Average experience.",
                            DateTime = DateTime.Now.AddDays(7),
                            BookingId = bookings[7].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 2,
                            Content = "Could be better.",
                            DateTime = DateTime.Now.AddDays(8),
                            BookingId = bookings[8].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 1,
                            Content = "Very poor service.",
                            DateTime = DateTime.Now.AddDays(9),
                            BookingId = bookings[9].Id
                        }
                    );
                    await context.SaveChangesAsync();
                }

                

                // Find CarOwner1 by NationalIdNo
                var carOwner1 = context.CarOwners.FirstOrDefault(co => co.NationalIdNo == "1234567890");

                if (carOwner1 != null && context.Cars.Count(c => c.CarOwnerId == carOwner1.Id) <= 5)
                {
                    context.Cars.AddRange(
                        new Car
                        {
                            Id = Guid.NewGuid(),
                            Name = "Mazda 3",
                            LicensePlate = "MAZ123",
                            Brand = "Mazda",
                            Model = "3",
                            Color = "Red",
                            NumberOfSeats = 5,
                            ProductionYears = 2021,
                            TransmissionType = "Automatic",
                            FuelType = "Gasoline",
                            Mileage = 5000,
                            FuelConsumption = 7.2,
                            BasePrice = 27000,
                            Deposit = 600,
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            Images = "img/mazda.jpg",
                            CarOwnerId = carOwner1.Id
                        },
                        new Car
                        {
                            Id = Guid.NewGuid(),
                            Name = "Kia Cerato",
                            LicensePlate = "KIA123",
                            Brand = "Kia",
                            Model = "Cerato",
                            Color = "Blue",
                            NumberOfSeats = 5,
                            ProductionYears = 2020,
                            TransmissionType = "Manual",
                            FuelType = "Diesel",
                            Mileage = 15000,
                            FuelConsumption = 6.5,
                            BasePrice = 24000,
                            Deposit = 500,
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            Images = "img/kia.jpg",
                            CarOwnerId = carOwner1.Id
                        },
                        new Car
                        {
                            Id = Guid.NewGuid(),
                            Name = "Hyundai Elantra",
                            LicensePlate = "HYU123",
                            Brand = "Hyundai",
                            Model = "Elantra",
                            Color = "Black",
                            NumberOfSeats = 5,
                            ProductionYears = 2019,
                            TransmissionType = "Automatic",
                            FuelType = "Gasoline",
                            Mileage = 20000,
                            FuelConsumption = 8.0,
                            BasePrice = 26000,
                            Deposit = 550,
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            Images = "img/hyundai.jpg",
                            CarOwnerId = carOwner1.Id
                        },
                        new Car
                        {
                            Id = Guid.NewGuid(),
                            Name = "Ford Ranger",
                            LicensePlate = "FRD123",
                            Brand = "Ford",
                            Model = "Ranger",
                            Color = "White",
                            NumberOfSeats = 5,
                            ProductionYears = 2018,
                            TransmissionType = "Manual",
                            FuelType = "Diesel",
                            Mileage = 25000,
                            FuelConsumption = 9.0,
                            BasePrice = 32000,
                            Deposit = 700,
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            Images = "img/ford_ranger.jpg",
                            CarOwnerId = carOwner1.Id
                        },
                        new Car
                        {
                            Id = Guid.NewGuid(),
                            Name = "Tesla Model 3",
                            LicensePlate = "TES123",
                            Brand = "Tesla",
                            Model = "Model 3",
                            Color = "Silver",
                            NumberOfSeats = 5,
                            ProductionYears = 2021,
                            TransmissionType = "Automatic",
                            FuelType = "Electric",
                            Mileage = 10000,
                            FuelConsumption = 0,
                            BasePrice = 60000,
                            Deposit = 1500,
                            Address = "Thành phố Hà Nội|Quận Cầu Giấy|Phường Trung Hoà",
                            Images = "img/tesla.jpg",
                            CarOwnerId = carOwner1.Id
                        }
                    );
                    await context.SaveChangesAsync();
                }
                if (carOwner1 != null && context.Bookings.Count(b => b.Car.CarOwnerId == carOwner1.Id) <= 3)
                {
                    var carIds = context.Cars.Where(c => c.CarOwnerId == carOwner1.Id).Select(c => c.Id).ToList();
                    var customers = context.Customers.Take(10).ToList();

                    context.Bookings.AddRange(
                                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-1",
                            StartDateTime = DateTime.Now,
                            EndDateTime = DateTime.Now.AddDays(5),
                            DriversInformation = "Driver 1",
                            PaymentMethod = "Credit Card",
                            Status = "Completed",
                            CustomerId = customers[0].Id,
                            CarId = carIds[0],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-2",
                            StartDateTime = DateTime.Now.AddDays(6),
                            EndDateTime = DateTime.Now.AddDays(10),
                            DriversInformation = "Driver 2",
                            PaymentMethod = "PayPal",
                            Status = "Completed",
                            CustomerId = customers[1].Id,
                            CarId = carIds[0],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-3",
                            StartDateTime = DateTime.Now.AddDays(11),
                            EndDateTime = DateTime.Now.AddDays(15),
                            DriversInformation = "Driver 3",
                            PaymentMethod = "Debit Card",
                            Status = "Completed",
                            CustomerId = customers[2].Id,
                            CarId = carIds[1],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-4",
                            StartDateTime = DateTime.Now.AddDays(16),
                            EndDateTime = DateTime.Now.AddDays(20),
                            DriversInformation = "Driver 4",
                            PaymentMethod = "Cash",
                            Status = "Completed",
                            CustomerId = customers[3].Id,
                            CarId = carIds[1],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-5",
                            StartDateTime = DateTime.Now.AddDays(21),
                            EndDateTime = DateTime.Now.AddDays(25),
                            DriversInformation = "Driver 5",
                            PaymentMethod = "Credit Card",
                            Status = "Completed",
                            CustomerId = customers[4].Id,
                            CarId = carIds[2],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-6",
                            StartDateTime = DateTime.Now.AddDays(26),
                            EndDateTime = DateTime.Now.AddDays(30),
                            DriversInformation = "Driver 6",
                            PaymentMethod = "PayPal",
                            Status = "Completed",
                            CustomerId = customers[5].Id,
                            CarId = carIds[2],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-7",
                            StartDateTime = DateTime.Now.AddDays(31),
                            EndDateTime = DateTime.Now.AddDays(35),
                            DriversInformation = "Driver 7",
                            PaymentMethod = "Credit Card",
                            Status = "Completed",
                            CustomerId = customers[6].Id,
                            CarId = carIds[3],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-8",
                            StartDateTime = DateTime.Now.AddDays(36),
                            EndDateTime = DateTime.Now.AddDays(40),
                            DriversInformation = "Driver 8",
                            PaymentMethod = "Debit Card",
                            Status = "Completed",
                            CustomerId = customers[7].Id,
                            CarId = carIds[3],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-9",
                            StartDateTime = DateTime.Now.AddDays(41),
                            EndDateTime = DateTime.Now.AddDays(45),
                            DriversInformation = "Driver 9",
                            PaymentMethod = "Credit Card",
                            Status = "Completed",
                            CustomerId = customers[8].Id,
                            CarId = carIds[4],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-10",
                            StartDateTime = DateTime.Now.AddDays(46),
                            EndDateTime = DateTime.Now.AddDays(50),
                            DriversInformation = "Driver 10",
                            PaymentMethod = "PayPal",
                            Status = "Completed",
                            CustomerId = customers[9].Id,
                            CarId = carIds[4],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-11",
                            StartDateTime = DateTime.Now.AddDays(51),
                            EndDateTime = DateTime.Now.AddDays(55),
                            DriversInformation = "Driver 11",
                            PaymentMethod = "Cash",
                            Status = "Completed",
                            CustomerId = customers[0].Id,
                            CarId = carIds[5],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-12",
                            StartDateTime = DateTime.Now.AddDays(56),
                            EndDateTime = DateTime.Now.AddDays(60),
                            DriversInformation = "Driver 12",
                            PaymentMethod = "Credit Card",
                            Status = "Completed",
                            CustomerId = customers[1].Id,
                            CarId = carIds[5],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-13",
                            StartDateTime = DateTime.Now.AddDays(61),
                            EndDateTime = DateTime.Now.AddDays(65),
                            DriversInformation = "Driver 13",
                            PaymentMethod = "PayPal",
                            Status = "Completed",
                            CustomerId = customers[2].Id,
                            CarId = carIds[0],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-14",
                            StartDateTime = DateTime.Now.AddDays(66),
                            EndDateTime = DateTime.Now.AddDays(70),
                            DriversInformation = "Driver 14",
                            PaymentMethod = "Credit Card",
                            Status = "Pending deposit",
                            CustomerId = customers[3].Id,
                            CarId = carIds[1],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-15",
                            StartDateTime = DateTime.Now.AddDays(71),
                            EndDateTime = DateTime.Now.AddDays(75),
                            DriversInformation = "Driver 15",
                            PaymentMethod = "PayPal",
                            Status = "In-progress",
                            CustomerId = customers[4].Id,
                            CarId = carIds[2],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-16",
                            StartDateTime = DateTime.Now.AddDays(76),
                            EndDateTime = DateTime.Now.AddDays(80),
                            DriversInformation = "Driver 16",
                            PaymentMethod = "Cash",
                            Status = "Pending payment",
                            CustomerId = customers[5].Id,
                            CarId = carIds[3],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-17",
                            StartDateTime = DateTime.Now.AddDays(81),
                            EndDateTime = DateTime.Now.AddDays(85),
                            DriversInformation = "Driver 17",
                            PaymentMethod = "Credit Card",
                            Status = "Completed",
                            CustomerId = customers[6].Id,
                            CarId = carIds[4],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-18",
                            StartDateTime = DateTime.Now.AddDays(86),
                            EndDateTime = DateTime.Now.AddDays(90),
                            DriversInformation = "Driver 18",
                            PaymentMethod = "PayPal",
                            Status = "Cancelled",
                            CustomerId = customers[7].Id,
                            CarId = carIds[5],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-19",
                            StartDateTime = DateTime.Now.AddDays(91),
                            EndDateTime = DateTime.Now.AddDays(95),
                            DriversInformation = "Driver 19",
                            PaymentMethod = "Debit Card",
                            Status = "Completed",
                            CustomerId = customers[8].Id,
                            CarId = carIds[0],
                            FeedbackId = null
                        },
                        new Booking
                        {
                            Id = Guid.NewGuid(),
                            BookingNo = "10072024-20",
                            StartDateTime = DateTime.Now.AddDays(96),
                            EndDateTime = DateTime.Now.AddDays(100),
                            DriversInformation = "Driver 20",
                            PaymentMethod = "Cash",
                            Status = "Completed",
                            CustomerId = customers[9].Id,
                            CarId = carIds[1],
                            FeedbackId = null
                        }
                    );
                    await context.SaveChangesAsync();
                }

                if (carOwner1 != null && context.Bookings.Any(b => b.Status == "Completed" && b.Car.CarOwnerId == carOwner1.Id) && context.Feedbacks.Count() <= 11)
                {
                    var completedBookings = context.Bookings.Where(b => b.Status == "Completed" && b.Car.CarOwnerId == carOwner1.Id).ToList();

                    context.Feedbacks.AddRange(
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 5,
                            Content = "Excellent service with Mazda 3!",
                            DateTime = DateTime.Now,
                            BookingId = completedBookings[0].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 4,
                            Content = "Very good experience with Kia Cerato.",
                            DateTime = DateTime.Now.AddDays(1),
                            BookingId = completedBookings[1].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 5,
                            Content = "Loved driving the Hyundai Elantra.",
                            DateTime = DateTime.Now.AddDays(2),
                            BookingId = completedBookings[2].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 4,
                            Content = "Ford Ranger was perfect for my trip.",
                            DateTime = DateTime.Now.AddDays(3),
                            BookingId = completedBookings[3].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 3,
                            Content = "Tesla Model 3 was good, but could be better.",
                            DateTime = DateTime.Now.AddDays(4),
                            BookingId = completedBookings[4].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 5,
                            Content = "Fantastic experience with Ford Ranger.",
                            DateTime = DateTime.Now.AddDays(5),
                            BookingId = completedBookings[5].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 4,
                            Content = "Good overall service with Hyundai Elantra.",
                            DateTime = DateTime.Now.AddDays(6),
                            BookingId = completedBookings[6].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 5,
                            Content = "Excellent condition of Kia Cerato.",
                            DateTime = DateTime.Now.AddDays(7),
                            BookingId = completedBookings[7].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 4,
                            Content = "Tesla Model 3 is a great car!",
                            DateTime = DateTime.Now.AddDays(8),
                            BookingId = completedBookings[8].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 5,
                            Content = "Amazing drive with Ford Ranger!",
                            DateTime = DateTime.Now.AddDays(9),
                            BookingId = completedBookings[9].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 3,
                            Content = "Hyundai Elantra was decent.",
                            DateTime = DateTime.Now.AddDays(10),
                            BookingId = completedBookings[10].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 4,
                            Content = "Great service, would rent the Tesla Model 3 again.",
                            DateTime = DateTime.Now.AddDays(11),
                            BookingId = completedBookings[11].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 5,
                            Content = "Mazda 3 was in excellent condition!",
                            DateTime = DateTime.Now.AddDays(12),
                            BookingId = completedBookings[12].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 5,
                            Content = "Loved the Ford Ranger, will rent again!",
                            DateTime = DateTime.Now.AddDays(13),
                            BookingId = completedBookings[13].Id
                        },
                        new Feedback
                        {
                            Id = Guid.NewGuid(),
                            Ratings = 4,
                            Content = "Kia Cerato provided a smooth ride.",
                            DateTime = DateTime.Now.AddDays(14),
                            BookingId = completedBookings[14].Id
                        }
                    );
                    await context.SaveChangesAsync();
                }
                //Add Feedback back to Bookings
                if (context.Bookings.Any() && context.Feedbacks.Any() && context.Bookings.All(b => b.FeedbackId == null))
                {
                    var feedbackList = context.Feedbacks.ToList();
                    foreach (Feedback feedback in feedbackList)
                    {
                        var booking = context.Bookings.FirstOrDefault(b => b.Id == feedback.BookingId);
                        if (booking != null)
                        {
                            booking.FeedbackId = feedback.Id;
                        }
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.Admins.Any())
                {
                    context.Admins.AddAsync(new Admin
                    {
                        Id = Guid.NewGuid(),
                        Email = "duc@gmail.com",
                        Name = "Trần Đức",
                        PasswordHash = HashPassword("Password123!")
                    });
                    await context.SaveChangesAsync();
                }

                //Thêm data từ Excel cho Address
                if (!context.AddressCities.Any() && !context.AddressDistricts.Any() && !context.AddressWards.Any())
                {
                    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    var relativePath = Path.Combine(baseDirectory, "..", "..", "..", "..", "RentalCar.Server", "Data", "AddressValueList.xls");
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                    using var stream = File.Open(relativePath, FileMode.Open, FileAccess.Read);
                    using var reader = ExcelReaderFactory.CreateReader(stream);
                    var result = reader.AsDataSet();

                    var dataTable = result.Tables[0];

                    var cities = new Dictionary<string, AddressCity>();
                    var districts = new Dictionary<string, AddressDistrict>();

                    for (int row = 1; row < dataTable.Rows.Count; row++)
                    {
                        var wardCode = dataTable.Rows[row][0].ToString();
                        var wardName = dataTable.Rows[row][1].ToString();
                        var districtCode = dataTable.Rows[row][2].ToString();
                        var districtName = dataTable.Rows[row][3].ToString();
                        var cityCode = dataTable.Rows[row][4].ToString();
                        var cityName = dataTable.Rows[row][5].ToString();

                        if (!cities.ContainsKey(cityCode))
                        {
                            var city = new AddressCity
                            {
                                Id = Guid.NewGuid(),
                                Name = cityName
                            };
                            cities.Add(cityCode, city);
                            context.AddressCities.Add(city);
                        }

                        if (!districts.ContainsKey(districtCode))
                        {
                            var district = new AddressDistrict
                            {
                                Id = Guid.NewGuid(),
                                Name = districtName,
                                CityId = cities[cityCode].Id
                            };
                            districts.Add(districtCode, district);
                            context.AddressDistricts.Add(district);
                        }

                        var ward = new AddressWard
                        {
                            Id = Guid.NewGuid(),
                            Name = wardName,
                            DistrictId = districts[districtCode].Id
                        };

                        context.AddressWards.Add(ward);
                    }

                    await context.SaveChangesAsync();
                }


                //Thêm data từ Excel cho Brand và Model
                if (!context.Brands.Any() && !context.Models.Any())
                {
                    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    var relativePath = Path.Combine(baseDirectory, "..", "..", "..", "..", "RentalCar.Server", "Data", "BrandModelList.xls");
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                    using var stream = File.Open(relativePath, FileMode.Open, FileAccess.Read);
                    using var reader = ExcelReaderFactory.CreateReader(stream);
                    var result = reader.AsDataSet();

                    var dataTable = result.Tables[0];

                    var brands = new Dictionary<string, Brand>();

                    for (int row = 1; row < dataTable.Rows.Count; row++)
                    {
                        var brandName = dataTable.Rows[row][1].ToString();
                        var modelName = dataTable.Rows[row][2].ToString();

                        if (!brands.ContainsKey(brandName))
                        {
                            var brand = new Brand
                            {
                                Id = Guid.NewGuid(),
                                Name = brandName
                            };
                            brands.Add(brandName, brand);
                            context.Brands.Add(brand);
                        }

                        var model = new Model
                        {
                            Id = Guid.NewGuid(),
                            Name = modelName,
                            BrandId = brands[brandName].Id
                        };

                        context.Models.Add(model);
                    }

                    await context.SaveChangesAsync();
                }
            }
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
