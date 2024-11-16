using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCar.Model
{
        public class FeedbackCarNameDTOModel
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

            public string CarName { get; set; }

    }
}
