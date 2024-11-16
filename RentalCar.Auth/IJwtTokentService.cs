using RentalCar.Entity.Entities;
using System.Security.Claims;

namespace RentalCar.Auth
{
    public interface IJwtTokenService
    {
        Task<string> GenerateAccessToken(Customer customer);
        Task<string> GenerateAccessToken(Admin admin);
        Task<string> GenerateAccessToken(CarOwner carOwner);
        Task<string> GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token); 
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
