using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RentalCar.Server.Controllers
{
    public class TokenCleanupService : IHostedService, IDisposable
    {
        private Timer _refreshTokenTimer;
        private Timer _resetTokenTimer;
        private readonly IConfiguration _configuration;

        public TokenCleanupService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _refreshTokenTimer = new Timer(CleanupExpiredRefreshTokens, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

            _resetTokenTimer = new Timer(CleanupExpiredResetTokens, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));

            return Task.CompletedTask;
        }

        private void CleanupExpiredRefreshTokens(object state)
        {
            var expirationTime = DateTime.UtcNow.AddDays(-double.Parse(_configuration["Jwt:RefreshTokenExpireDays"]));
            AuthController._refreshTokens.RemoveAll(rt => rt.CreatedAt <= expirationTime);
        }

        private void CleanupExpiredResetTokens(object state)
        {
            var resetTokenExpirationTime = DateTime.UtcNow.AddMinutes(-2); 
            AuthController._resetTokens.RemoveAll(rt => rt.CreatedAt <= resetTokenExpirationTime);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _refreshTokenTimer?.Change(Timeout.Infinite, 0);
            _resetTokenTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _refreshTokenTimer?.Dispose();
            _resetTokenTimer?.Dispose();
        }
    }
}