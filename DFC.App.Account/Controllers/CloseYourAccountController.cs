using System;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.Auth.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.APP.Account.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DFC.App.Account.Controllers
{
    [Authorize]
    public class CloseYourAccountController : CompositeSessionController<CloseYourAccountCompositeViewModel>
    {
        private readonly IOpenIDConnectClient _openIdConnectClient;
        private readonly IDocumentService<CmsApiSharedContentModel> _documentService;
        private readonly Guid _sharedContent;
        private readonly ILogger<CloseYourAccountController> _logger;

        public CloseYourAccountController(IOptions<CompositeSettings> compositeSettings, IAuthService authService,IOpenIDConnectClient openIdConnectClient, IDocumentService<CmsApiSharedContentModel> documentService, IConfiguration config, ILogger<CloseYourAccountController> logger)
            : base(compositeSettings, authService, documentService, config)
        {
            _openIdConnectClient = openIdConnectClient;
            _documentService = documentService;
            _sharedContent = config.GetValue<Guid>("SharedContentGuid");
            _logger = logger;
        }

        public override async Task<IActionResult> Body()
        {
            var customer = await GetCustomerDetails();
            ViewModel.CustomerId = customer.CustomerId;
            return await base.Body();
        }

        [HttpPost]
        [Route("/body/close-your-account")]
        public  async Task<IActionResult> Body(CloseYourAccountCompositeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = $"Error: {ViewModel.PageTitle}";
                _logger.LogInformation($"CloseYourAccountController /body/close-your-account PageTitle {ViewModel.PageTitle}" );
                return View(ViewModel);
            }

            var customer = await GetCustomerDetails();

            if (_openIdConnectClient.VerifyPassword(customer.Contact.EmailAddress, model.Password).Result.IsFailure)
            {
                _logger.LogWarning($"CloseYourAccountController /body/close-your-account Wrong password");
                ModelState.AddModelError("Password", "Wrong password. Try again.");
                return View(ViewModel);
            }

            ViewModel.PageTitle = $"Are you sure you want to close your account? | {ViewModel.PageTitle}";
            var sharedContent = await _documentService.GetByIdAsync(_sharedContent, "account").ConfigureAwait(false);
            ViewModel.SharedSideBar = sharedContent?.Content;
            _logger.LogInformation($"CloseYourAccountController /body/close-your-account PageTitle {ViewModel.PageTitle} confirmed closing of account");
            return base.View("ConfirmDeleteAccount", ViewModel);
        }
        

    }
}
