using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
            //var userId = user.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

            //if (userId == null)
            //{
            //    throw new NoUserIdInClaimException("Unable to locate userID");
            //}

            //only needed while we stub the Auth Will throw error if unknown customer id

            string userId = _httpContextAccessor.HttpContext.Request.Query["customerid"].ToString()??"";

            userId = userId == "" ? "07f05f9b-fdf4-4d2d-93f2-a43f99c95a91" : userId;
                            

            return await _dssReader.GetCustomerData(userId);
        }
    }
}
