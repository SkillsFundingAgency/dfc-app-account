using System.Linq;
using System.Threading.Tasks;
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
        [Route("/body/authsuccess")]
        public override async Task<IActionResult> Body()
        {
            Request.Query.TryGetValue("redirectUrl", out var redirectUrl);
            var url = string.IsNullOrEmpty(redirectUrl.ToList().FirstOrDefault())
                ? "/your-account"
                : redirectUrl.ToList().FirstOrDefault();
            return Redirect(url);
        }

       
    }
}
