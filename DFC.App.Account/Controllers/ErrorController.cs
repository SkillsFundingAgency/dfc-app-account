using DFC.APP.Account.Data.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.ViewModels;
//using DFC.Compui.Cosmos.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;

namespace DFC.App.Account.Controllers
{

    public class ErrorController : CompositeSessionController<ErrorCompositeViewModel>
    {
        private readonly ILogger<ErrorController> _logger;
        private readonly ISharedContentRedisInterface sharedContentRedis;
        public ErrorController(ILogger<ErrorController> logger, IOptions<CompositeSettings> compositeSettings, IAuthService authService, IConfiguration config, ISharedContentRedisInterface sharedContentRedis)
            : base(compositeSettings, authService, config, sharedContentRedis)
        {
            _logger = logger;
            this.sharedContentRedis = sharedContentRedis;;
        }

        
    }
}