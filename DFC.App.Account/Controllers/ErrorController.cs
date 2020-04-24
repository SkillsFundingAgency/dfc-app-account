using System.Threading.Tasks;
using DFC.App.Account.Models;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.Account.Controllers
{
    
    public class ErrorController : CompositeSessionController<ErrorCompositeViewModel>
    {
        private readonly ILogger<ErrorController> _logger;
        public ErrorController(ILogger<ErrorController> logger, IOptions<CompositeSettings> compositeSettings)
            :base(compositeSettings)
        {
            _logger = logger;
        }

        public override async Task<IActionResult> Body()
        {
            return await base.Body();
        }
    }
}