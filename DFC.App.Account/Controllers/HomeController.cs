using Dfc.ProviderPortal.Packages;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.SHC.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Threading.Tasks;
using DFC.APP.Account.Data.Models;
//using DFC.Compui.Cosmos.Contracts;
using Microsoft.Extensions.Configuration;

namespace DFC.App.Account.Controllers
{
    public class HomeController : CompositeSessionController<HomeCompositeViewModel>
    {
        private readonly ISkillsHealthCheckService _skillsHealthCheckService;
        private readonly AuthSettings _authSettings;
        private readonly IDssReader _dssReader;
        private readonly ActionPlansSettings _actionPlansSettings;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IAuthService authService, IDssReader dssReader, ISkillsHealthCheckService skillsHealthCheckService, IOptions<AuthSettings> authSettings, IOptions<ActionPlansSettings> actionPlansSettings,  IConfiguration config)
            : base(compositeSettings, authService, config)
        {
            Throw.IfNull(skillsHealthCheckService, nameof(skillsHealthCheckService));
            _skillsHealthCheckService = skillsHealthCheckService;
            _authSettings = authSettings.Value;
            _dssReader = dssReader;
            _actionPlansSettings = actionPlansSettings.Value;
            _logger = logger;
        }

        #region Default Routes
        // The home page uses MVC default routes, so we need non "/[controller]" attribute routed versions of the endpoints just for here
        [Route("/head/{controller}/{id?}")]
        [Route("/head/{id?}")]
        public override IActionResult Head()
        {
            return base.Head();
        }
        [Route("/bodytop/{controller}/{id?}")]
        [Route("/bodytop/{id?}")]
        public override IActionResult BodyTop()
        {
            return base.BodyTop();
        }
        [Route("/breadcrumb/{controller}/{id?}")]
        [Route("/breadcrumb/{id?}")]
        public override IActionResult Breadcrumb()
        {
            return base.Breadcrumb();
        }
        [Authorize]
        [Route("/body/{controller}/{id?}")]
        [Route("/body/{id?}")]

        public override async Task<IActionResult> Body()
        {
            var customer = await GetCustomerDetails();
            ViewModel.ResetPasswordUrl = _authSettings.ResetPasswordUrl;
            ViewModel.ActionPlansUrl = _actionPlansSettings.Url;
            ViewModel.ActionPlans = await _dssReader.GetActionPlans(customer.CustomerId.ToString());
            return await base.Body();
        }
        [Route("/bodyfooter/{controller}/{id?}")]
        [Route("/bodyfooter/{id?}")]


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
        [Route("/body/{controller}/register")]
        public IActionResult Register()
        {
            return Redirect(_authSettings.RegisterUrl + "?redirecturl=/your-account");
        }

        // It's important to use the same URL route as the existing Skills Health Check app in production
        [Route("/your-account/home/signin")]
        [Route("/body/{controller}/signin")]
        public IActionResult SignIn()
        {
            return Redirect(_authSettings.SignInUrl+"?redirecturl=/your-account");
        }

        // It's important to use the same URL route as the existing Skills Health Check app in production
        [Route("/your-account/home/signout")]
        [Route("/body/{controller}/signout")]
        public IActionResult SignOut(bool accountClosed)
        {
            var redirectUrl = accountClosed
                ? $"{_authSettings.SignOutUrl}?redirectUrl={_authSettings.Issuer}/your-account/Delete-Account/AccountClosed"
                : _authSettings.SignOutUrl;
            _logger.LogInformation($"accountClosed: {accountClosed},  Redirecting to {redirectUrl}");
            return Redirect(redirectUrl);
        }
        #endregion
    }
}
