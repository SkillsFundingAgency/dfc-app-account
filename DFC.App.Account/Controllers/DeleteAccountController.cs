﻿using DFC.App.Account.Application.Common.Constants;
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
    [Authorize]
    public class DeleteAccountController : CompositeSessionController<DeleteAccountCompositeViewModel>
    {
        private readonly IDssWriter _dssService;
        public DeleteAccountController(IOptions<CompositeSettings> compositeSettings, IDssWriter dssService, IAuthService authService)
            : base(compositeSettings, authService)
        {
            _dssService = dssService;
        }

        [HttpGet, HttpPost]
        public async Task<IActionResult> Body(DeleteAccountCompositeViewModel model)
        {

             var deleteCustomerRequest = new DeleteCustomerRequest()
             {
                 CustomerId = model.CustomerId,
                 DateOfTermination = DateTime.UtcNow,
                 ReasonForTermination = Constants.ClosureReasonCustomerChoice
             };
             await _dssService.DeleteCustomer(deleteCustomerRequest);
             return View(ViewModel);
        }

    }
}
