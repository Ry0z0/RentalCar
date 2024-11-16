using System.ComponentModel.DataAnnotations;

namespace RentalCar.Model
{
    public class BookingModel
    {
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

        [Required]
        public Guid CarId { get; set; }

        public Guid? FeedbackId { get; set; }

    }
}
