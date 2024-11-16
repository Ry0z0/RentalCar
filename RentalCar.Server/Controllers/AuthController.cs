using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalCar.Auth;
using RentalCar.Entity.Contexts;
using RentalCar.Entity.Entities;
using RentalCar.Model;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;

namespace RentalCar.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly RentalCarDbContext _context;
        private readonly ICustomerService _customerService;
        private readonly ICarOwnerService _carOwnerService;
        private readonly IConfiguration _configuration;
        public static List<RefreshRequest> _refreshTokens = new List<RefreshRequest>();
        public static List<ResetTokenEntry> _resetTokens = new List<ResetTokenEntry>();

        public AuthController(IJwtTokenService jwtTokenService, RentalCarDbContext context, ICustomerService customerService, ICarOwnerService carOwnerService, IConfiguration configuration)
        {
            _jwtTokenService = jwtTokenService;
            _context = context;
            _customerService = customerService;
            _carOwnerService = carOwnerService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = _context.Customers.FirstOrDefault(c => c.Email == loginRequest.Email);
            if (customer != null)
            {
                if (customer.PasswordHash == loginRequest.Password)
                {
                    var accessToken = await _jwtTokenService.GenerateAccessToken(customer);
                    var refreshToken = _jwtTokenService.GenerateRefreshToken();
                    var token = new RefreshRequest
                    {
                        Token = accessToken,
                        RefreshToken = refreshToken,
                        Email = customer.Email,
                        CreatedAt = DateTime.UtcNow // Set the creation time
                    };
                    _refreshTokens.Add(token);
                    customer.PasswordHash = ""; // Clear password hash before sending to client

                    return Ok(new { Token = token, User = customer, Role = "customer", UserId = customer.Id });
                }
                return Unauthorized(new { Message = "Invalid password" });
            }

            var carOwner = _context.CarOwners.FirstOrDefault(c => c.Email == loginRequest.Email);
            if (carOwner != null)
            {
                if (carOwner.PasswordHash == loginRequest.Password)
                {
                    var accessToken = await _jwtTokenService.GenerateAccessToken(carOwner);
                    var refreshToken = _jwtTokenService.GenerateRefreshToken();
                    var token = new RefreshRequest
                    {
                        Token = accessToken,
                        RefreshToken = refreshToken,
                        Email = carOwner.Email,
                        CreatedAt = DateTime.UtcNow // Set the creation time
                    };
                    _refreshTokens.Add(token);
                    carOwner.PasswordHash = ""; // Clear password hash before sending to client

                    return Ok(new { Token = token, User = carOwner, Role = "carOwner", UserId = carOwner.Id });
                }
                return Unauthorized(new { Message = "Invalid password" });
            }

            var admin = _context.Admins.FirstOrDefault(a => a.Email == loginRequest.Email);
            if (admin != null)
            {
                if (admin.PasswordHash == loginRequest.Password)
                {
                    var accessToken = await _jwtTokenService.GenerateAccessToken(admin);
                    var refreshToken = _jwtTokenService.GenerateRefreshToken();
                    var token = new RefreshRequest
                    {
                        Token = accessToken,
                        RefreshToken = refreshToken,
                        Email = admin.Email,
                        CreatedAt = DateTime.UtcNow // Set the creation time
                    };
                    _refreshTokens.Add(token);
                    admin.PasswordHash = ""; // Clear password hash before sending to client

                    return Ok(new { Token = token, User = admin, Role = "admin", UserId = admin.Id });
                }
                return Unauthorized(new { Message = "Invalid password" });
            }

            return NotFound(new { Message = "User not found" });
        }


        [AllowAnonymous]
        [HttpPost("signup/customer")]
        public async Task<IActionResult> SignUpCustomer([FromBody] CustomerModel signUpRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.Customers.Any(c => c.Email == signUpRequest.Email))
            {
                return Conflict(new { Message = "Email already exists" });
            }

            signUpRequest.Id = Guid.NewGuid();
            signUpRequest.PasswordHash = signUpRequest.PasswordHash;

            await _customerService.AddAsync(signUpRequest);
            return Ok(new { Message = "Customer created successfully" });
        }

        [AllowAnonymous]
        [HttpPost("signup/carowner")]
        public async Task<IActionResult> SignUpCarOwner([FromBody] CarOwnerModel signUpRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.CarOwners.Any(c => c.Email == signUpRequest.Email))
            {
                return Conflict(new { Message = "Email already exists" });
            }

            signUpRequest.Id = Guid.NewGuid();
            signUpRequest.PasswordHash = signUpRequest.PasswordHash;

            await _carOwnerService.AddAsync(signUpRequest);
            
            return Ok(new { Message = "Car owner created successfully" });
        }

        [Authorize]
        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] RefreshRequest refreshRequest)
        {
            var existingToken = _refreshTokens.FirstOrDefault(rt => rt.RefreshToken == refreshRequest.RefreshToken && rt.Email == refreshRequest.Email);

            if (existingToken == null || existingToken.CreatedAt.AddDays(_configuration.GetValue<int>("Jwt:RefreshTokenExpireDays")) < DateTime.UtcNow)
            {
                return Unauthorized(new { Message = "Invalid or expired refresh token" });
            }

            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(refreshRequest.Token);
            var newToken = _jwtTokenService.GenerateAccessToken(principal.Claims).Result;
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            existingToken.Token = newToken;
            existingToken.RefreshToken = newRefreshToken;
            existingToken.CreatedAt = DateTime.UtcNow; // Update the creation time

            return Ok(new
            {
                Token = newToken,
                RefreshToken = newRefreshToken
            });
        }


        //"/api/Auth/checkPassword"
        [AllowAnonymous]
        [HttpPost("checkPassword")]
        public IActionResult CheckPassword([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Try to find the user in the Customers table
            var customer = _context.Customers.FirstOrDefault(c => c.Email == loginRequest.Email);

            // If not found, try to find the user in the CarOwners table
            var carOwner = _context.CarOwners.FirstOrDefault(co => co.Email == loginRequest.Email);

            // If both are null, return NotFound
            if (customer == null && carOwner == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            // Use the appropriate user object based on which was found
            var userPasswordHash = customer?.PasswordHash ?? carOwner?.PasswordHash;

            var hashedPassword = loginRequest.Password;
            if (userPasswordHash != hashedPassword)
            {
                return Unauthorized(new { Message = "Invalid password" });
            }

            return Ok(new { Message = "Password is correct" });
        }



        [AllowAnonymous]
        [HttpPost("forgetPassword")]
        public IActionResult ForgetPassword([FromBody] EmailRequest request)
        {
            var email = request.Email;

            // Try to find the user in the Customers table
            var customer = _context.Customers.FirstOrDefault(u => u.Email == email);

            // If not found, try to find the user in the CarOwners table
            var carOwner = _context.CarOwners.FirstOrDefault(u => u.Email == email);

            // If both are null, return NotFound
            if (customer == null && carOwner == null)
            {
                return NotFound(new { Message = "Email does not exist" });
            }

            // Generate OTP
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();

            // Send OTP to the user's email
            SendOtpEmail(email, otp);

            // Create a new OtpEntry and store it in session
            var otpEntry = new OtpEntry { Email = email, Otp = otp };
            HttpContext.Session.SetString("otpEntry", Newtonsoft.Json.JsonConvert.SerializeObject(otpEntry));

            return Ok(new { Message = "OTP has been sent to your email" });
        }



        [AllowAnonymous]
        [HttpPost("verifyOtp")]
        public IActionResult VerifyOtp([FromBody] OtpEntry otpEntry)
        {
            string sessionOtpEntryString = HttpContext.Session.GetString("otpEntry");

            if (string.IsNullOrEmpty(sessionOtpEntryString))
            {
                return BadRequest(new { Message = "No OTP found. Please request a new OTP." });
            }

            var sessionOtpEntry = Newtonsoft.Json.JsonConvert.DeserializeObject<OtpEntry>(sessionOtpEntryString);

            if (otpEntry.Email != sessionOtpEntry.Email || otpEntry.Otp != sessionOtpEntry.Otp)
            {
                return BadRequest(new { Message = "Invalid OTP or email" });
            }

            // OTP is valid, generate a temporary token for password change
            var token = Guid.NewGuid().ToString(); // Generate a unique token

            // Store the token and email in the static list
            _resetTokens.Add(new ResetTokenEntry { Email = otpEntry.Email, Token = token });

            // Remove OTP from session
            HttpContext.Session.Remove("otpEntry");

            return Ok(new { Message = "OTP verified. Please reset your password.", Token = token });
        }



        [AllowAnonymous]
        [HttpPost("changePassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            // Find the token in the static list
            var resetTokenEntry = _resetTokens.FirstOrDefault(rt => rt.Token == request.Token && rt.Email == request.Email);

            if (resetTokenEntry == null)
            {
                return Unauthorized(new { Message = "Invalid token" });
            }

            var customer = _context.Customers.FirstOrDefault(u => u.Email == request.Email);
            var carOwner = _context.CarOwners.FirstOrDefault(u => u.Email == request.Email);

            if (customer == null && carOwner == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            // Hash the new password
            var hashedPassword = request.NewPassword;

            if (customer != null)
            {
                customer.PasswordHash = hashedPassword;
            }
            else if (carOwner != null)
            {
                carOwner.PasswordHash = hashedPassword;
            }

            if (_context.SaveChanges() > 0)
            {
                _resetTokens.Remove(resetTokenEntry);
                return Ok(new { Message = "Password changed successfully!" });
            }
            else
            {
                return StatusCode(500, new { Message = "An error occurred while changing the password." });
            }
        }


        [AllowAnonymous]
        [HttpPost("changePasswordProfile")]
        public IActionResult ChangePasswordProfile([FromBody] ChangePasswordRequest request)
        {
            // Find the token in the static list
            var resetTokenEntry = _refreshTokens.FirstOrDefault(rt => rt.Token == request.Token && rt.Email == request.Email);

            if (resetTokenEntry == null)
            {
                return Unauthorized(new { Message = "Invalid token" });
            }

            var customer = _context.Customers.FirstOrDefault(u => u.Email == request.Email);
            var carOwner = _context.CarOwners.FirstOrDefault(u => u.Email == request.Email);

            if (customer == null && carOwner == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            // Hash the new password
            var hashedPassword = request.NewPassword;

            if (customer != null)
            {
                customer.PasswordHash = hashedPassword;
            }
            else if (carOwner != null)
            {
                carOwner.PasswordHash = hashedPassword;
            }

            if (_context.SaveChanges() > 0)
            {
                // Remove the used token from the static list

                return Ok(new { Message = "Password changed successfully!" });
            }
            else
            {
                return StatusCode(500, new { Message = "An error occurred while changing the password." });
            }
        }

        private void SendOtpEmail(string recipientEmail, string otp)
        {
            try
            {
                var fromAddress = new MailAddress(_configuration["Smtp:Username"], "RentalCarApp");
                var toAddress = new MailAddress(recipientEmail);
                string fromPassword = _configuration["Smtp:Password"];
                string subject = "Your OTP Code";
                string body = $"Your OTP code is: {otp}." +
                              $"It will expire after 2 minutes. Do not share for anyone!";


                var smtp = new SmtpClient
                {
                    Host = _configuration["Smtp:Host"],
                    Port = int.Parse(_configuration["Smtp:Port"]),
                    EnableSsl = bool.Parse(_configuration["Smtp:EnableSsl"]),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout([FromBody] LogOutRequest refreshToken)
        {
            // Find the refresh token in the list
            var tokenEntry = _refreshTokens.FirstOrDefault(rt => rt.RefreshToken == refreshToken.refreshToken);

            if (tokenEntry == null)
            {
                return NotFound(new { Message = "Refresh token not found" });
            }

            // Remove the token from the list
            _refreshTokens.Remove(tokenEntry);

            return Ok(new { Message = "Logged out successfully" });
        }


        [HttpGet("GetAllToken")]
        public IActionResult GetAllToken()
        {
            return Ok(new { ListToken = _refreshTokens });
        }


        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return System.Convert.ToBase64String(hash);
            }
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }


    public class RefreshRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Add CreatedAt timestamp
    }


    public class LogOutRequest
    {
        public string refreshToken { get; set; }
    }
    public class EmailRequest
    {
        public string Email { get; set; }
    }
    public class OtpEntry
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
    public class ChangePasswordRequest
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
    public class ResetTokenEntry
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
