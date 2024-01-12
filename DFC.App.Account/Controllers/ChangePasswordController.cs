using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.APP.Account.Data.Models;
//using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;


namespace DFC.App.Account.Controllers
{
    [Authorize]
    public class ChangePasswordController : CompositeSessionController<ChangePasswordCompositeViewModel>
    {
        public ChangePasswordController(IOptions<CompositeSettings> compositeSettings, IAuthService authService,  IConfiguration config)
            : base(compositeSettings, authService, config)
        {
            
        }
        public override async Task<IActionResult> Body()
        {
            return await base.Body();
        }
    }
}