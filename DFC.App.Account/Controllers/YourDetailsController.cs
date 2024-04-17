using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.APP.Account.Data.Models;
using Microsoft.Extensions.Configuration;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;

namespace DFC.App.Account.Controllers
{

    [Authorize]
    public class YourDetailsController : CompositeSessionController<YourDetailsCompositeViewModel>
    {

        private readonly IDssReader _dssService;
        private readonly ISharedContentRedisInterface sharedContentRedis;

        public YourDetailsController(ILogger<YourDetailsController> logger, IOptions<CompositeSettings> compositeSettings, IDssReader dssService, IAuthService authService,IConfiguration config, ISharedContentRedisInterface sharedContentRedis)
            : base(compositeSettings, authService, config, sharedContentRedis)
        {
            _dssService = dssService;
            this.sharedContentRedis = sharedContentRedis;
        }

        
      [Route("/body/your-details")] 
        public override async Task<IActionResult> Body()
        {
            var customer = await GetCustomerDetails();
            ViewModel.CustomerDetails = await _dssService.GetCustomerData(customer.CustomerId.ToString());
            return await base.Body();
        }
    }
}