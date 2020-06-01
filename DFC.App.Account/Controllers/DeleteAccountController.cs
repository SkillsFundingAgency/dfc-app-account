using DFC.App.Account.Application.Common.Constants;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

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

        [Authorize]
        [HttpGet]
        [Route("/body/delete-account")]
        public async Task<IActionResult> Body()
        {
            return RedirectTo($"{CompositeViewModel.PageId.Home}");
        }
        [Authorize]
        [HttpPost]
        [Route("/body/delete-account")]
        public async Task<IActionResult> Body(DeleteAccountCompositeViewModel model)
        {
            var customer = await GetCustomerDetails();

            if (customer.CustomerId != model.CustomerId)
            {
                return RedirectTo($"{CompositeViewModel.PageId.Home}");
            }

            var deleteCustomerRequest = new DeleteCustomerRequest()
            {
                CustomerId = customer.CustomerId,
                DateOfTermination = DateTime.UtcNow,
                ReasonForTermination = Constants.ClosureReasonCustomerChoice
            };
            await _dssService.DeleteCustomer(deleteCustomerRequest);
            return RedirectTo($"{CompositeViewModel.PageId.Home}/signOut?redirect=/your-account/Delete-Account/AccountClosed");
        }
        
        [HttpGet]
        [Route("/body/delete-account/AccountClosed")]
        public async Task<IActionResult> AccountClosed()
        {
            return await base.Body();
        }

    }
}
