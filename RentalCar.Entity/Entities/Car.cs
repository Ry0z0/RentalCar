using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalCar.Entity.Entities
{
    [Table("Car", Schema = "instance")]
    public class Car
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string LicensePlate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Brand { get; set; }

        [Required]
        [MaxLength(50)]
        public string Model { get; set; }

        [MaxLength(20)]
        public string Color { get; set; }

        [Range(1, 50)]
        public int NumberOfSeats { get; set; }

        [Range(1900, int.MaxValue)]
        public int ProductionYears { get; set; }

        [MaxLength(50)]
        public string TransmissionType { get; set; }

        [MaxLength(50)]
        public string FuelType { get; set; }

        [Range(0, int.MaxValue)]
        public int Mileage { get; set; }

        [Range(0, double.MaxValue)]
        public double FuelConsumption { get; set; }

        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal BasePrice { get; set; }

        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal Deposit { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(1000)]
        public string? AdditionalFunctions { get; set; }

        [MaxLength(1000)]
        public string? TermsOfUse { get; set; }

        [MaxLength(1000)]
        public string? Images { get; set; }

        [Required]
        public bool Active { get; set; } = true;

        public Guid CarOwnerId { get; set; }

        public virtual CarOwner CarOwner { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
