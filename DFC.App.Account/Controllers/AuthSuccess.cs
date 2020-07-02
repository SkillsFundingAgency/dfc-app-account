using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.Account.Controllers
{
    public class AuthSuccess: CompositeSessionController<AuthSuccessCompositeViewModel>
    {
        public AuthSuccess(IOptions<CompositeSettings> compositeSettings, IAuthService authService)
            : base(compositeSettings, authService)
        {
            
        }
        [Authorize]
        [Route("/authsuccess/{url}")]
        public IActionResult RedirectUrl(string url)
        {
            return Redirect(url);
        }

       
    }
}
