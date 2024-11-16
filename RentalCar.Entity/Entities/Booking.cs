using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalCar.Entity.Entities
{
    [Table("Booking", Schema = "instance")]
    public class Booking
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string BookingNo { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDateTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDateTime { get; set; }

        [MaxLength(1000)]
        public string? DriversInformation { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }
        
        [Required]
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [Required]
        public Guid CarId { get; set; }
        public Car Car { get; set; }

        public Guid? FeedbackId { get; set; }
        public virtual Feedback Feedback { get; set; }
    }
}
