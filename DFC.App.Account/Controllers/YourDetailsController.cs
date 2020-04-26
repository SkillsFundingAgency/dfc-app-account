﻿using System.Threading.Tasks;
using DFC.App.Account.Models;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.Account.Controllers
{
 
     [Route("your-details")]
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
        
        
      [Route("/body/your-details")] 
        public  async Task<IActionResult> Body(string customerId)
        {
            customerId= customerId ?? "ac78e0b9-950a-407a-9f99-51dc63ce699a";
            ViewModel.CustomerDetails = await _dssService.GetCustomerData(customerId);
            return View(ViewModel);
        }
    }
}