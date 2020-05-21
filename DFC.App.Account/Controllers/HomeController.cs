﻿using Dfc.ProviderPortal.Packages;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.SHC.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DFC.App.Account.Controllers
{
    public class HomeController : CompositeSessionController<HomeCompositeViewModel>
    {
        private readonly ISkillsHealthCheckService _skillsHealthCheckService;
        private readonly AuthSettings _authSettings;
        public HomeController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IAuthService authService, ISkillsHealthCheckService skillsHealthCheckService, IOptions<AuthSettings> authSettings)
        :base(compositeSettings, authService)
        {
            Throw.IfNull(skillsHealthCheckService, nameof(skillsHealthCheckService));
            _skillsHealthCheckService = skillsHealthCheckService;
            _authSettings = authSettings.Value;
        }

        #region Default Routes
        [Authorize]
        // The home page uses MVC default routes, so we need non "/[controller]" attribute routed versions of the endpoints just for here
        [Route("/head/{controller}")]
        [Route("/head")]
        public override IActionResult Head()
        {
            return base.Head();
        }
        [Authorize]
        [Route("/bodytop/{controller}")]
        [Route("/bodytop")]
        public override IActionResult BodyTop()
        {
            return base.BodyTop();
        }
        [Authorize]
        [Route("/breadcrumb/{controller}")]
        [Route("/breadcrumb")]
        public override IActionResult Breadcrumb()
        {
            return base.Breadcrumb();
        }
        [Authorize]
        [Route("/body/{controller}")]
        [Route("/body")]

        public override async Task<IActionResult> Body()
        {
            //Hard coded value - Needs removing upon account, and DSS integration
            ViewModel.ShcDocuments = _skillsHealthCheckService.GetShcDocumentsForUser("200010216");
            return await base.Body();
        }
        [Authorize]
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
        public IActionResult SignOut()
        {
            return Redirect(_authSettings.SignOutUrl);
        }
        #endregion
    }
}
