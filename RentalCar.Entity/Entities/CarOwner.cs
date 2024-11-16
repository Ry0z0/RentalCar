using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalCar.Entity.Entities
{
    [Table("CarOwner", Schema = "instance")]
    public class CarOwner
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(20)]
        public string?NationalIdNo { get; set; }

        [Required]
        [Phone]
        [MaxLength(15)]
        public string PhoneNo { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string PasswordHash { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(50)]
        public string? DrivingLicense { get; set; }

        [MaxLength(100)]
        public string? Wallet { get; set; }

        public virtual ICollection<Car> Cars { get; set; }


    }
}
