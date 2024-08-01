using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.Auth.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.SharedHtml;
using Constants = DFC.Common.SharedContent.Pkg.Netcore.Constant.ApplicationKeys;

namespace DFC.App.Account.Controllers
{
    [Authorize]
    public class CloseYourAccountController : CompositeSessionController<CloseYourAccountCompositeViewModel>
    {
        private const string ExpiryAppSettings = "Cms:Expiry";
        private readonly IConfiguration configuration;
        private readonly IOpenIDConnectClient _openIdConnectClient;
        private readonly ISharedContentRedisInterface sharedContentRedisInterface;
        private string status = string.Empty;
        private double expiryInHours = 4;


        public CloseYourAccountController(IOptions<CompositeSettings> compositeSettings, IAuthService authService,IOpenIDConnectClient openIdConnectClient, IConfiguration config, ISharedContentRedisInterface _sharedContentRedisInterface)
            : base(compositeSettings, authService,  config, _sharedContentRedisInterface)
        {
            _openIdConnectClient = openIdConnectClient;
            this.configuration=config;
            this.sharedContentRedisInterface = _sharedContentRedisInterface;

            status = configuration.GetSection("ContentMode:ContentMode").Get<string>();
            if (this.configuration != null)
            {
                string expiryAppString = this.configuration.GetSection(ExpiryAppSettings).Get<string>();
                if (double.TryParse(expiryAppString, out var expiryAppStringParseResult))
                {
                    expiryInHours = expiryAppStringParseResult;
                }
            }
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
                return View(ViewModel);
            }

            var customer = await GetCustomerDetails();

            if (_openIdConnectClient.VerifyPassword(customer.Contact.EmailAddress, model.Password).Result.IsFailure)
            {
                ModelState.AddModelError("Password", "Wrong password. Try again.");
                return View(ViewModel);
            }

            if (string.IsNullOrEmpty(status))
            {
                status = "PUBLISHED";
            }

            var sharedhtml = await sharedContentRedisInterface.GetDataAsyncWithExpiry<SharedHtml>(Constants.SpeakToAnAdviserSharedContent, status, expiryInHours);


            ViewModel.PageTitle = $"Are you sure you want to close your account? | {ViewModel.PageTitle}";
            ViewModel.SharedSideBar = sharedhtml.Html;
            return base.View("ConfirmDeleteAccount", ViewModel);
        }
    }
}
