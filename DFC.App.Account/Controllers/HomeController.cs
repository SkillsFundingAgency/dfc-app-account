using Dfc.ProviderPortal.Packages;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.SHC.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DFC.App.Account.Controllers
{
    public class HomeController : CompositeSessionController<HomeCompositeViewModel>
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISkillsHealthCheckService _skillsHealthCheckService;
        public HomeController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IAuthService authService, ISkillsHealthCheckService skillsHealthCheckService)
        :base(compositeSettings, authService)
        {
            _logger = logger;
            Throw.IfNull(skillsHealthCheckService, nameof(skillsHealthCheckService));
            _skillsHealthCheckService = skillsHealthCheckService;
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
    }
}
