using DFC.APP.Account.Data.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.ViewModels;
//using DFC.Compui.Cosmos.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.Account.Controllers
{

    public class ErrorController : CompositeSessionController<ErrorCompositeViewModel>
    {
        private readonly ILogger<ErrorController> _logger;
        public ErrorController(ILogger<ErrorController> logger, IOptions<CompositeSettings> compositeSettings, IAuthService authService, IConfiguration config)
            : base(compositeSettings, authService, config)
        {
            _logger = logger;
        }

        
    }
}