using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.App.Account.Services.SHC.Interfaces;
using Dfc.ProviderPortal.Packages;
using Microsoft.Extensions.Logging;

namespace DFC.App.Account.Controllers
{
    public class ConfirmDeleteController : CompositeSessionController<ConfirmDeleteCompositeViewModel>
    {
        private readonly ISkillsHealthCheckService _skillsHealthCheckService;
        public ConfirmDeleteController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IAuthService authService, ISkillsHealthCheckService skillsHealthCheckService)
            : base(compositeSettings, authService)
        {
            Throw.IfNull(skillsHealthCheckService, nameof(skillsHealthCheckService));
            _skillsHealthCheckService = skillsHealthCheckService;
        }

        public override async Task<IActionResult> Body()
        {
            return await base.Body();
        }
        [HttpGet]
        [Route("/body/[controller]/{*id}")]
        public async Task<IActionResult> Body(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectPermanent(CompositeViewModel.PageId.Home.Value);
            }

            return await base.Body();
        }
    }
}