using System.Threading.Tasks;
using DFC.App.Account.Models;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.Account.Controllers
{
    
    public class YourDetailsController : CompositeSessionController<YourDetailsCompositeViewModel>
    {
        private readonly ILogger<YourDetailsController> _logger;
        private readonly IDssReader _dssService;

        public YourDetailsController(ILogger<YourDetailsController> logger, IOptions<CompositeSettings> compositeSettings, IDssReader dssService)
        :base(compositeSettings)
        {
            _logger = logger;
            _dssService = dssService;
        }

        public  async Task<IActionResult> Body(string customerId)
        {
            //var customerId = "993cfb94-12b7-41c4-b32d-7be9331174f1";
            customerId= customerId ?? "ac78e0b9-950a-407a-9f99-51dc63ce699a";//With Address
            ViewModel.CustomerDetails = await _dssService.GetCustomerData(customerId);
            return View(ViewModel);
        }

        #region DefaultRegions

       [Route("/head/your-details")]
       [Route("/head")]
        public override IActionResult Head()
        { 
            return base.Head();
        }
        [Route("/bodytop/your-details")]
        [Route("/bodytop")]
        public override IActionResult BodyTop()
        {
            return base.BodyTop();
        }
        [Route("/breadcrumb/your-details")]
        [Route("/breadcrumb")]
        public override IActionResult Breadcrumb()
        {
            return base.Breadcrumb();
        }

        #endregion
    }
}