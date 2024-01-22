using System.Threading.Tasks;
using DFC.APP.Account.Data.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.ViewModels;
//using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;

namespace DFC.App.Account.Controllers
{
    public class SessionTimeoutController : CompositeSessionController<SessionTimeoutCompositeViewModel>
    {
        private readonly AuthSettings _authSettings;
        private readonly ISharedContentRedisInterface sharedContentRedis;
        public SessionTimeoutController(IOptions<CompositeSettings> compositeSettings, IAuthService authService, IOptions<AuthSettings> authSettings,IConfiguration config, ISharedContentRedisInterface sharedContentRedis)
            : base(compositeSettings, authService, config, sharedContentRedis)
        {
            _authSettings = authSettings.Value;
            this.sharedContentRedis = sharedContentRedis;


        }

        public override async Task<IActionResult> Body()
        {
            ViewModel.SignInUrl = _authSettings.SignInUrl;
            return await base.Body();
        }

    }
}