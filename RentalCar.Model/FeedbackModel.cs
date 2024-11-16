using System.ComponentModel.DataAnnotations;

namespace RentalCar.Model
{
    public class FeedbackModel
    {
        public Guid Id { get; set; }

        [Range(1, 5)]
        public int Ratings { get; set; }

        [MaxLength(1000)]
        public string? Content { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }

        public Guid BookingId { get; set; }

    }
}
