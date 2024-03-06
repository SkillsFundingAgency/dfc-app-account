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

namespace DFC.App.Account.Controllers
{
    [Authorize]
    public class CloseYourAccountController : CompositeSessionController<CloseYourAccountCompositeViewModel>
    {
        private readonly IOpenIDConnectClient _openIdConnectClient;
        private new const string SharedContentStaxId = "2c9da1b3-3529-4834-afc9-9cd741e59788";
        private readonly ISharedContentRedisInterface sharedContentRedisInterface;
        private string status = string.Empty;


        public CloseYourAccountController(IOptions<CompositeSettings> compositeSettings, IAuthService authService,IOpenIDConnectClient openIdConnectClient, IConfiguration config, ISharedContentRedisInterface _sharedContentRedisInterface)
            : base(compositeSettings, authService,  config, _sharedContentRedisInterface)
        {
            _openIdConnectClient = openIdConnectClient;
            this.sharedContentRedisInterface = _sharedContentRedisInterface;
            status = config.GetConnectionString("ContentMode:ContentMode");
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

            var sharedhtml = await sharedContentRedisInterface.GetDataAsync<SharedHtml>("SharedContent/" + SharedContentStaxId, status);


            ViewModel.PageTitle = $"Are you sure you want to close your account? | {ViewModel.PageTitle}";
            ViewModel.SharedSideBar = sharedhtml.Html;
            return base.View("ConfirmDeleteAccount", ViewModel);
                      
         
        }
        

    }
}
