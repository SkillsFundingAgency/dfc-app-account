﻿using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.APP.Account.Data.Models;
using Microsoft.Extensions.Configuration;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;

namespace DFC.App.Account.Controllers
{

    public class DeleteAccountController : CompositeSessionController<DeleteAccountCompositeViewModel>
    {
        private readonly IDssWriter _dssService;
        private readonly ISharedContentRedisInterface sharedContentRedis;
        public DeleteAccountController(IOptions<CompositeSettings> compositeSettings, IDssWriter dssService, IAuthService authService, IConfiguration config, ISharedContentRedisInterface sharedContentRedis)
            : base(compositeSettings, authService, config, sharedContentRedis)
        {
            _dssService = dssService;
            this.sharedContentRedis = sharedContentRedis;
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

            await _dssService.DeleteCustomer(customer.CustomerId);
            return RedirectTo($"{CompositeViewModel.PageId.Home}/signOut?accountClosed=true");
        }
        
        [HttpGet]
        [Route("/body/delete-account/AccountClosed")]
        public async Task<IActionResult> AccountClosed()
        {
            return await base.Body();
        }

    }
}
