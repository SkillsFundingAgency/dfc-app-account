using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.APP.Account.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.SharedHtml;
using DFC.Common.SharedContent.Pkg.Netcore;

namespace DFC.App.Account.Controllers
{
    [Authorize]
    public class ChangePasswordController : CompositeSessionController<ChangePasswordCompositeViewModel>
    {
        private readonly ISharedContentRedisInterface sharedContentRedis;
        public ChangePasswordController(IOptions<CompositeSettings> compositeSettings, IAuthService authService,  IConfiguration config, ISharedContentRedisInterface sharedContentRedis)
            : base(compositeSettings, authService, config, sharedContentRedis)
        {

            this.sharedContentRedis = sharedContentRedis;
        }
        public override async Task<IActionResult> Body()
        {
            return await base.Body();
        }
    }
}