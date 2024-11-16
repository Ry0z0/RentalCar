using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCar.Entity.Entities
{
    [Table("AddressDistrict", Schema = "reference")]
    public class AddressDistrict
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid CityId { get; set; }
        public AddressCity City { get; set; }
        public ICollection<AddressWard>? Wards { get; set; }

    }
}
