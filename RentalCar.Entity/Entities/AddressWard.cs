using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCar.Entity.Entities
{
    [Table("AddressWard", Schema = "reference")]
    public class AddressWard
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid DistrictId { get; set; }
        public AddressDistrict District { get; set; }
    }
}
