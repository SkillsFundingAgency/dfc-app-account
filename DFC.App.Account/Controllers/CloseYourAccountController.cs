using System.Net;
using DFC.App.Account.Models;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.App.Account.Services;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;

namespace DFC.App.Account.Controllers
{
   
    public class CloseYourAccountController : CompositeSessionController<CloseYourAccountCompositeViewModel>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private ISession _session => _contextAccessor.HttpContext.Session;

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
            
            ViewModel.PageTitle = $"Are you sure you want to close your account? | {ViewModel.PageTitle}";
            return View("ConfirmDeleteAccount",ViewModel);
        }
        

    }
}
