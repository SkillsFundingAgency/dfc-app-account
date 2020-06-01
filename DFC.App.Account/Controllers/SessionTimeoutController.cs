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
        private readonly AuthSettings _authSettings;
        public SessionTimeoutController(IOptions<CompositeSettings> compositeSettings, IAuthService authService, IOptions<AuthSettings> authSettings)
            : base(compositeSettings, authService)
        {
            _authSettings = authSettings.Value;
        }

        public override async Task<IActionResult> Body()
        {
            
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    ViewModel.SignInUrl = _authSettings.SignInUrl;
                    return await base.Body();
                }
                else
                {
                    return RedirectTo(_authSettings.SignInUrl);
                }

            }
            catch (System.Exception ex)
            {
                return RedirectTo(_authSettings.SignInUrl);
            }
        }

    }
}