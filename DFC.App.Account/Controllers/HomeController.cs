using Dfc.ProviderPortal.Packages;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.AzureB2CAuth.Interfaces;
using DFC.App.Account.Services.SHC.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.Account.Controllers
{
    public class HomeController : CompositeSessionController<HomeCompositeViewModel>
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISkillsHealthCheckService _skillsHealthCheckService;
        private readonly IOpenIDConnectClient _authClient;

        public HomeController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IAuthService authService, ISkillsHealthCheckService skillsHealthCheckService, IOpenIDConnectClient authClient)
        :base(compositeSettings, authService)
        {
            _logger = logger;
            Throw.IfNull(skillsHealthCheckService, nameof(skillsHealthCheckService));
            _skillsHealthCheckService = skillsHealthCheckService;
            _authClient = authClient;
        }

        #region Default Routes

        // The home page uses MVC default routes, so we need non "/[controller]" attribute routed versions of the endpoints just for here
        [Route("/head/{controller}")]
        [Route("/head")]
        public override IActionResult Head()
        {
            return base.Head();
        }
        [Route("/bodytop/{controller}")]
        [Route("/bodytop")]
        public override IActionResult BodyTop()
        {
            return base.BodyTop();
        }
        [Route("/breadcrumb/{controller}")]
        [Route("/breadcrumb")]
        public override IActionResult Breadcrumb()
        {
            return base.Breadcrumb();
        }

        [Route("/body/{controller}")]
        [Route("/body")]

        public override async Task<IActionResult> Body()
        {
            var x = await GetCustomerDetails();
            //Hard coded value - Needs removing upon account, and DSS integration
            ViewModel.ShcDocuments = _skillsHealthCheckService.GetShcDocumentsForUser("200010216");
            return await base.Body();
        }

        [Route("/bodyfooter/{controller}")]
        [Route("/bodyfooter")]

        public override IActionResult BodyFooter()
        {
            return base.BodyFooter();
        }
        #endregion Default Routes

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Auth Routes

        // It's important to use the same URL route as the existing Skills Health Check app in production
        [Route("/your-account/home/register")]
        public async Task<IActionResult> Register()
        {
            var registerUrl = await _authClient.GetRegisterUrl();
            return Redirect(registerUrl);
        }

        // It's important to use the same URL route as the existing Skills Health Check app in production
        [Route("/your-account/home/signin")]
        public async Task<IActionResult> SignIn()
        {
            var signInUrl = await _authClient.GetSignInUrl();
            return Redirect(signInUrl);
        }

        // Set this route up in the Azure B2C config on the Azure Portal as the redirect Url
        // This is where we receive the token upon a successful user authentication
        [Route("/your-account/auth")]
        public async Task<IActionResult> Auth(string id_token, string state)
        {
            JwtSecurityToken validatedToken;
            try
            {
                validatedToken = await _authClient.ValidateToken(id_token);
            }
            catch (System.Exception ex)
            {
                // TBC: how to handle invalid token?
                _logger.LogError(ex, "Failed to validate auth token.");
                throw;
            }

            // TBC: How to use validated token now - what claims to stuff in the session?
            //      Which claim is the unique ID of the user - is it TID?
            // For now we'll temporarily stuff the users' full name in the redirect Url just to demonstrate we are logged in
            var userFullName = validatedToken.Claims.First(claim => claim.Type == "name").Value;
            var loggedInUrl = _authClient.GetAuthdUrl();
            return Redirect($"{loggedInUrl}?UserFullName={userFullName}");
        }

        #endregion
    }
}
