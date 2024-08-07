﻿using System.Linq;
using System.Threading.Tasks;
using DFC.APP.Account.Data.Models;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;

namespace DFC.App.Account.Controllers
{
    public class AuthSuccess: CompositeSessionController<AuthSuccessCompositeViewModel>
    {
        private readonly IDssWriter _dssWriter;
        private readonly ISharedContentRedisInterface sharedContentRedis;
        public AuthSuccess(IOptions<CompositeSettings> compositeSettings, IAuthService authService, IDssWriter dssWriter,  IConfiguration config, ISharedContentRedisInterface sharedContentRedis)
            : base(compositeSettings, authService,  config, sharedContentRedis)
        {
            _dssWriter = dssWriter;
            this.sharedContentRedis = sharedContentRedis;
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
