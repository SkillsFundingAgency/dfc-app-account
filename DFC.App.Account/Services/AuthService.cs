using System.Linq;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using DFC.App.Account.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DFC.App.Account.Services
{
    public class AuthService : IAuthService
    {
        private readonly IDssReader _dssReader;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IDssReader dssReader,IHttpContextAccessor httpContextAccessor)
        {
            _dssReader = dssReader;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Customer> GetCustomer(ClaimsPrincipal user)
        {
            var userId = user.Claims.FirstOrDefault(x => x.Type == "CustomerId")?.Value;

            if (userId == null)
            {
                throw new NoUserIdInClaimException("Unable to locate userID");
            }




            return await _dssReader.GetCustomerData(userId);
        }
    }
}
