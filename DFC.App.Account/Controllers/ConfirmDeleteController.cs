using Dfc.ProviderPortal.Packages;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.SHC.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using DFC.APP.Account.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DFC.App.Account.Services.DSS.Models;

namespace DFC.App.Account.Controllers
{
    [Authorize]
    public class ConfirmDeleteController : CompositeSessionController<ConfirmDeleteCompositeViewModel>
    {
        private readonly ISkillsHealthCheckService _skillsHealthCheckService;
        private readonly ILogger<ConfirmDeleteController> _logger;

        public ConfirmDeleteController(IOptions<CompositeSettings> compositeSettings, IAuthService authService, ISkillsHealthCheckService skillsHealthCheckService, IDocumentService<CmsApiSharedContentModel> documentService, IConfiguration config, ILogger<ConfirmDeleteController> logger)
            : base(compositeSettings, authService, documentService, config)
        {
            Throw.IfNull(skillsHealthCheckService, nameof(skillsHealthCheckService));
            _skillsHealthCheckService = skillsHealthCheckService;
            _logger = logger;

        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public override async Task<IActionResult> Body()
        {
            return await base.Body();
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("/body/[controller]/{*id}")]
        public async Task<IActionResult> Body(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning($"ConfirmDeleteController Body id is null");
                return RedirectTo(CompositeViewModel.PageId.Home.Value);
            }

            ViewModel.DocumentId = id;

            var customer = await GetCustomerDetails();

            //Hard coded value - Needs removing upon account, and DSS integration
            var shcDocuments = _skillsHealthCheckService.GetShcDocumentsForUser("200010216");

            if (!shcDocuments.Any())
                return RedirectTo(CompositeViewModel.PageId.Home.Value);

            var doc = shcDocuments.FirstOrDefault(x => x.DocumentId == ViewModel.DocumentId);

            if (doc == null)
            {
                _logger.LogInformation($"ConfirmDeleteController Body customer doc is null ");
                return RedirectTo(CompositeViewModel.PageId.Home.Value);
            }

            ViewModel.CreatedAt = doc.CreatedAt;
            return await base.Body();
        }

        [Route("/body/[controller]")]
        [HttpPost]
        public IActionResult Body(ConfirmDeleteCompositeViewModel viewModel)
        {
            if (viewModel == null)
            {
                _logger.LogWarning($"ConfirmDeleteController Body/controller viewModel is null ");
                return RedirectTo(CompositeViewModel.PageId.Home.Value);
            }

            if (string.IsNullOrWhiteSpace(viewModel.DocumentId))
            {
                _logger.LogWarning($"ConfirmDeleteController Body/controller DocumentId is null ");
                return RedirectTo(CompositeViewModel.PageId.Home.Value);
            }

            //Code to delete SHC goes here.

            _logger.LogInformation($"ConfirmDeleteController Body/controller DocumentId is {viewModel.DocumentId} ");
            return RedirectTo($"{CompositeViewModel.PageId.ShcDeleted.Value}?id={viewModel.DocumentId}");
        }



    }
}