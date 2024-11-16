using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCar.Model
{
    public class SearchCarDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public int NumberOfSeats { get; set; }
        public int ProductionYears { get; set; }
        public string TransmissionType { get; set; }
        public string FuelType { get; set; }
        public int Mileage { get; set; }
        public double FuelConsumption { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Deposit { get; set; }
        public string Address { get; set; }
        public string? Description { get; set; }
        public string? AdditionalFunctions { get; set; }
        public string? TermsOfUse { get; set; }
        public string? Images { get; set; }
        public bool Active { get; set; }
        public Guid CarOwnerId { get; set; }
        public string? CarOwner { get; set; }
        public int NoOfRides { get; set; }
        public double Ratings { get; set; }
    }

}
