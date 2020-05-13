using System.Threading.Tasks;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.Account.Controllers
{
    public class SessionTimeoutController : CompositeSessionController<SessionTimeoutCompositeViewModel>
    {
        public SessionTimeoutController(IOptions<CompositeSettings> compositeSettings, IAuthService authService)
            : base(compositeSettings, authService)
        {
        }
        public override async Task<IActionResult> Body()
        {
            return await base.Body();
        }
    }
}