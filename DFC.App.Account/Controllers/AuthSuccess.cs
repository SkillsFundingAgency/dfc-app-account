using System.Linq;
using System.Threading.Tasks;
using DFC.APP.Account.Data.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.ViewModels;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DFC.App.Account.Controllers
{
    public class AuthSuccess: CompositeSessionController<AuthSuccessCompositeViewModel>
    {
        private readonly IDssWriter _dssWriter;
        public AuthSuccess(IOptions<CompositeSettings> compositeSettings, IAuthService authService, IDssWriter dssWriter, IDocumentService<CmsApiSharedContentModel> documentService, IConfiguration config)
            : base(compositeSettings, authService, documentService, config)
        {
            _dssWriter = dssWriter;
        }
        [Authorize]
        [Route("/body/authsuccess")]
        public override async Task<IActionResult> Body()
        {
            var customer = await GetCustomerDetails();
            if(customer == null)
            {
                return BadRequest("unable to get customer details");
            }
            var token = User.Claims.FirstOrDefault(x => x.Type == "DssToken");
            await _dssWriter.UpdateLastLogin(customer.CustomerId, token?.Value);

            Request.Query.TryGetValue("redirectUrl", out var redirectUrl);
            var url = string.IsNullOrEmpty(redirectUrl.ToList().FirstOrDefault())
                ? "/your-account"
                : redirectUrl.ToList().FirstOrDefault();
            return Redirect(url);
        }
       
    }
}
