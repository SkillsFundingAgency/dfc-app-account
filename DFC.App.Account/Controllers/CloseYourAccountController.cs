using DFC.App.Account.Models;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.Account.Controllers
{
    public class CloseYourAccountController : CompositeSessionController<CloseYourAccountCompositeViewModel>
    {
        public CloseYourAccountController(IOptions<CompositeSettings> compositeSettings) : base(compositeSettings)
        {
            
        }
        [Route("/body/close-your-account")] 
        public override async Task<IActionResult> Body()
        {
            return View(ViewModel);
        }
    }
}