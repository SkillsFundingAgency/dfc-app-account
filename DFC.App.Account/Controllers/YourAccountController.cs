using DFC.App.Account.Models;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.Account.Controllers
{
    public class YourAccountController : CompositeSessionController<YourAccountCompositeViewModel>
    {
        public YourAccountController(ILogger logger, IOptions<CompositeSettings> compositeSettings) : base(logger, compositeSettings)
        {
            
        }
        public override async Task<IActionResult> Body()
        {
            return View();
        }
    }
}