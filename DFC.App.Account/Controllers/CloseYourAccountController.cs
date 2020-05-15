using DFC.App.Account.Models;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.App.Account.Application.Common.Constants;
using DFC.App.Account.Services;
using Microsoft.AspNetCore.Http;

namespace DFC.App.Account.Controllers
{
   
    public class CloseYourAccountController : CompositeSessionController<CloseYourAccountCompositeViewModel>
    {
        public CloseYourAccountController(IOptions<CompositeSettings> compositeSettings, IAuthService authService)
            : base(compositeSettings, authService)
        {
            
        }

        public override async Task<IActionResult> Body()
        {
            return View(ViewModel);
        }

        [HttpPost]
        public  IActionResult Body(CloseYourAccountCompositeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = $"Error: {ViewModel.PageTitle}";
                return View(ViewModel);
            }
            HttpContext.Session.SetString(Constants.SessionCustomerPasswordValidated, "true");  
            ViewModel.PageTitle = $"Are you sure you want to close your account? | {ViewModel.PageTitle}";
            return View("ConfirmDeleteAccount",ViewModel);
        }
        

    }
}
