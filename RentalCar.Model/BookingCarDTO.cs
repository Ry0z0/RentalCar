using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCar.Model
{
    public class BookingCarDTO
    {
        public string BookingNo { get; set; }
        public Guid CarId { get; set; }
        public Guid BookingId { get; set; }
        public string CarName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Deposit { get; set; }
        public string Status { get; set; }
    }
}
