using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCar.Model
{
    public class CarStatusDTO
    {
        public Guid CarId { get; set; }
        public string CarName { get; set; }
        public string Status { get; set; }
        public Guid? BookingId { get; set; }
    }

}
