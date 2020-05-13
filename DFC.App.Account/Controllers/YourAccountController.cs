using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.Account.Models;
using DFC.App.Account.Services.AzureB2CAuth.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.Account.Controllers
{
    public class YourAccountController : CompositeSessionController<YourDetailsCompositeViewModel>
    {
        private readonly IOpenIDConnectClient _authClient;
       
        public YourAccountController(ILogger<YourAccountController> logger, IOptions<CompositeSettings> compositeSettings, IOpenIDConnectClient authClient)
            : base(compositeSettings)
        {
            _authClient = authClient;
        }

        [Route("/your-account/home/register")]
        public async Task<IActionResult> Register()
        {
            var registerUrl = await _authClient.GetRegisterUrl();
            return Redirect(registerUrl);
        }

        [Route("/your-account/home/signin")]
        public async Task<IActionResult> SignIn()
        {
            var signInUrl = await _authClient.GetSignInUrl();
            return Redirect(signInUrl);
        }

        [Route("/your-account/home")]
        public async Task<IActionResult> Home(string id_token, string state)
        {
            JwtSecurityToken validatedToken = null;
            try
            {
                validatedToken = await _authClient.ValidateToken(id_token);
            }
            catch(System.Exception ex)
            {
                // TBC: how to handle invalid token?
                throw;
            }

            // TBC: How to use validated token now - what claims to stuff in the session?
            //      Which claim is the unique ID of the user - is it TID?
            //      Temporarily we pass the user full name to the view so we can demonstrate that auth succeeded. 
            var userFullName = validatedToken.Claims.First(claim => claim.Type == "name").Value;

            return View("Home", userFullName);
        }
    }
}