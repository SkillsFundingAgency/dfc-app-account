using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.Account.Controllers
{
    public class ConfirmDeleteController : CompositeSessionController<ConfirmDeleteCompositeViewModel>
    {
        public ConfirmDeleteController(IOptions<CompositeSettings> compositeSettings, IAuthService authService)
            : base(compositeSettings, authService)
        {

        }

        public override async Task<IActionResult> Body()
        {
            return await base.Body();
        }

    }
}