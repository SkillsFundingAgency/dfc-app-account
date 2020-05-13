using System.Linq;
using Dfc.ProviderPortal.Packages;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.SHC.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.Account.Controllers
{
    public class ConfirmDeleteController : CompositeSessionController<ConfirmDeleteCompositeViewModel>
    {
        private readonly ISkillsHealthCheckService _skillsHealthCheckService;
        public ConfirmDeleteController(IOptions<CompositeSettings> compositeSettings, IAuthService authService, ISkillsHealthCheckService skillsHealthCheckService)
            : base(compositeSettings, authService)
        {
            Throw.IfNull(skillsHealthCheckService, nameof(skillsHealthCheckService));
            _skillsHealthCheckService = skillsHealthCheckService;
        }
        [HttpGet]
        public override async Task<IActionResult> Body()
        {
            return await base.Body();
        }
        [HttpGet]
        [Route("/body/[controller]/{*id}")]
        public async Task<IActionResult> Body(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectTo(CompositeViewModel.PageId.Home.Value);

            ViewModel.DocumentId = id;

            var customer = await GetCustomerDetails();

            //Hard coded value - Needs removing upon account, and DSS integration
            var shcDocuments = _skillsHealthCheckService.GetShcDocumentsForUser("200010216");

            if (!shcDocuments.Any())
                return RedirectTo(CompositeViewModel.PageId.Home.Value);
            var doc = shcDocuments.FirstOrDefault(x => x.DocumentId == ViewModel.DocumentId);

            if(doc == null)
                return RedirectTo(CompositeViewModel.PageId.Home.Value);

            ViewModel.CreatedAt = doc.CreatedAt;
            return await base.Body();
        }

        
        

    }
}