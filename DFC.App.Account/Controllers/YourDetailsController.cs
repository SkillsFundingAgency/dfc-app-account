using System.Threading.Tasks;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.Account.Controllers
{
 
     
     public class YourDetailsController : CompositeSessionController<YourDetailsCompositeViewModel>
    {
        
        private readonly IDssReader _dssService;

        public YourDetailsController(ILogger<YourDetailsController> logger, IOptions<CompositeSettings> compositeSettings, IDssReader dssService, IAuthService authService)
            : base(compositeSettings, authService)
        {
            _dssService = dssService;
        }
        
        
      [Route("/body/your-details")] 
        public override async Task<IActionResult> Body()
        {
            var customer = await GetCustomerDetails();
            ViewModel.CustomerDetails = await _dssService.GetCustomerData(customer.CustomerId.ToString());
            return View(ViewModel);
        }
    }
}