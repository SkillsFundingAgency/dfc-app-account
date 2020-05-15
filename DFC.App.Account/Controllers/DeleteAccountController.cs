using System;
using DFC.App.Account.Models;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.App.Account.Application.Common.Constants;
using DFC.App.Account.Exception;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using Microsoft.AspNetCore.Http;

namespace DFC.App.Account.Controllers
{
    public class DeleteAccountController : CompositeSessionController<DeleteAccountCompositeViewModel>
    {
        private readonly IDssWriter _dssService;
        public DeleteAccountController(IOptions<CompositeSettings> compositeSettings, IDssWriter dssService, IAuthService authService)
            : base(compositeSettings, authService)
        {
            _dssService = dssService;
        }

        [HttpGet,HttpPost]
        public async Task<IActionResult> Body(DeleteAccountCompositeViewModel model)
        {
            
            var customerId= HttpContext.Session.GetString(Constants.SessionCustomerId);
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(Constants.SessionCustomerPasswordValidated)))
            {
                throw new UserNotValidatedException("Cant' deleted user, password not validated.");
            }
            
             var deleteCustomerRequest = new DeleteCustomerRequest()
             {
                 CustomerId = new Guid(customerId),
                 DateOfTermination = DateTime.UtcNow,
                 ReasonForTermination = "3"
             };
             await _dssService.DeleteCustomer(deleteCustomerRequest);
             return View(ViewModel);
        }

    }
}
