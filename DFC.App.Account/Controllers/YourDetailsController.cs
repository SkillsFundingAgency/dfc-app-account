using System.Threading.Tasks;
using DFC.App.Account.Application.Common.Constants;
using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Http;
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
        public  async Task<IActionResult> Body(string customerId)
        {
            customerId= customerId ?? "ac78e0b9-950a-407a-9f99-51dc63ce699a";
           // HttpContext.Session.SetString(Constants.SessionCustomerId, customerId);  
            ViewModel.CustomerDetails = await _dssService.GetCustomerData(customerId);
            return View(ViewModel);
        }
    }
}