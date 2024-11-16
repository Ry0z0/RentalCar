using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalCar.Entity.Entities
{
    [Table("Feedback", Schema = "instance")]
    public class Feedback
    {
        [Key]
        public Guid Id { get; set; }

        [Range(1, 5)]
        public int Ratings { get; set; }

        [MaxLength(1000)]
        public string? Content { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }

        public Guid BookingId { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
