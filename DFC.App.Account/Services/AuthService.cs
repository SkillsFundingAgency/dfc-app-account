using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DFC.App.Account.Services
{
    public class AuthService : IAuthService
    {
        private readonly IDssReader _dssReader;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthService> _logger;
        public AuthService(IDssReader dssReader, IHttpContextAccessor httpContextAccessor, ILogger<AuthService> logger)
        {
            _dssReader = dssReader;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<Customer> GetCustomer(ClaimsPrincipal user)
        {
            var userId = user.Claims.FirstOrDefault(x => x.Type == "CustomerId")?.Value;

            if (userId == null)
            {
                _logger.LogError("Unable to Locate User with {userId}", userId);
                return null;
            }

            return await _dssReader.GetCustomerData(userId);
        }
    }
}
