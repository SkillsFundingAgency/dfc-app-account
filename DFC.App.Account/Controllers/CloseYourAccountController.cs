using DFC.App.Account.Models;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.App.Account.Services;
using DFC.App.Account.Services.AzureB2CAuth.Interfaces;
using DFC.App.Account.Services.DSS.Models;

namespace DFC.App.Account.Controllers
{
   
    public class CloseYourAccountController : CompositeSessionController<CloseYourAccountCompositeViewModel>
    {
        private readonly IOpenIDConnectClient _openIdConnectClient;
        private readonly IAuthService _authService;
        public CloseYourAccountController(IOptions<CompositeSettings> compositeSettings, IAuthService authService,IOpenIDConnectClient openIdConnectClient)
            : base(compositeSettings, authService)
        {
            _openIdConnectClient = openIdConnectClient;
            _authService = authService;
        }

        public override async Task<IActionResult> Body()
        {
            return View(ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Body(CloseYourAccountCompositeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = $"Error: {ViewModel.PageTitle}";
                return View(ViewModel);
            }

            if (_openIdConnectClient.VerifyPassword("gbaryah@gmail.com", model.Password).Result.IsFailure)
            {
                ModelState.AddModelError("Password", "Wrong password. Try again.");
                return View(ViewModel);
            }
            
           // HttpContext.Session.SetString(Constants.SessionCustomerPasswordValidated, "true");

            ViewModel.PageTitle = $"Are you sure you want to close your account? | {ViewModel.PageTitle}";
            return View("ConfirmDeleteAccount", ViewModel);
        }
        

    }
}
