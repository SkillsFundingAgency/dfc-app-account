using System.Threading.Tasks;
using DFC.APP.Account.Data.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.ViewModels;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DFC.App.Account.Controllers
{
    public class SessionTimeoutController : CompositeSessionController<SessionTimeoutCompositeViewModel>
    {
        private readonly AuthSettings _authSettings;
        public SessionTimeoutController(IOptions<CompositeSettings> compositeSettings, IAuthService authService, IOptions<AuthSettings> authSettings, IDocumentService<CmsApiSharedContentModel> documentService, IConfiguration config)
            : base(compositeSettings, authService, documentService, config)
        {
            _authSettings = authSettings.Value;
        }

        public override async Task<IActionResult> Body()
        {
            ViewModel.SignInUrl = _authSettings.SignInUrl;
            return await base.Body();
        }

    }
}