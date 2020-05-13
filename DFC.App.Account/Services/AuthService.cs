using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DFC.App.Account.Services
{
    public class AuthService : IAuthService
    {
        private readonly IDssReader _dssReader;

        public AuthService(IDssReader dssReader)
        {
            _dssReader = dssReader;
        }

        public async Task<Customer> GetCustomer(ClaimsPrincipal user)
        {
            //var userId = user.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

            //if (userId == null)
            //{
            //    throw new NoUserIdInClaimException("Unable to locate userID");
            //}

            //only needed while we stub the Auth Will throw error if unknown customer id

           var  userId = "ac78e0b9-950a-407a-9f99-51dc63ce699a";

           return await _dssReader.GetCustomerData(userId);
        }
    }
}
