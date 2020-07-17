using System.Linq;
using System.Threading.Tasks;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.Account.Controllers
{
    public class AuthSuccess: CompositeSessionController<AuthSuccessCompositeViewModel>
    {
        private readonly IDssWriter _dssWriter;
        public AuthSuccess(IOptions<CompositeSettings> compositeSettings, IAuthService authService, IDssWriter dssWriter)
            : base(compositeSettings, authService)
        {
            _dssWriter = dssWriter;
        }
        [Authorize]
        [Route("/body/authsuccess")]
        public override async Task<IActionResult> Body()
        {
            await UpdateLastLoggedIn();
            Request.Query.TryGetValue("redirectUrl", out var redirectUrl);
            var url = string.IsNullOrEmpty(redirectUrl.ToList().FirstOrDefault())
                ? "/your-account"
                : redirectUrl.ToList().FirstOrDefault();
            return Redirect(url);
        }

        private async Task UpdateLastLoggedIn()
        {
            var customer = await GetCustomerDetails();
            await _dssWriter.UpdateLastLogin(customer.CustomerId);
        }
       
    }
}
