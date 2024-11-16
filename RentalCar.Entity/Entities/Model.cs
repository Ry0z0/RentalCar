using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCar.Entity.Entities
{
    [Table("Model", Schema = "reference")]
    public class Model
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }
    }
}
