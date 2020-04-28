using System.Security.Claims;
using System.Threading.Tasks;
using DFC.App.Account.Services.DSS.Models;

namespace DFC.App.Account.Services
{
    public interface IAuthService
    {
        public Task<Customer> GetCustomer(ClaimsPrincipal user);
    }
}
