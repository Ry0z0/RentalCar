using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalCar.Model
{
    public class UserDetailDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? NationalIdNo { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? Address { get; set; }
        public string? DrivingLicense { get; set; }
        public string? Wallet { get; set; }
        public string Role { get; set; } // 'customer' or 'carowner'
    }

}
