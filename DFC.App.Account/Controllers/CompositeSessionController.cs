using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.ViewModels;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.SharedHtml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Constants = DFC.Common.SharedContent.Pkg.Netcore.Constant.ApplicationKeys;

namespace DFC.App.Account.Controllers
{

    /// <summary>
    /// Adds default Composite UI endpoints and routing logic to the base session controller.
    /// </summary>
    public abstract class CompositeSessionController<TViewModel>:Controller where TViewModel : CompositeViewModel, new()
    {
        private const string ExpiryAppSettings = "Cms:Expiry";
        private readonly IAuthService _authService;
        private readonly IConfiguration configuration;
        private readonly ISharedContentRedisInterface sharedContentRedisInterface;
        private string status = string.Empty;
        private double expiry = 4;

        protected TViewModel ViewModel { get; }

        protected CompositeSessionController(IOptions<CompositeSettings> compositeSettings, IAuthService authService, IConfiguration config,ISharedContentRedisInterface _sharedContentRedisInterface)
        {
            _authService = authService;
            configuration = config;
            ViewModel = new TViewModel()
            {
                CompositeSettings = compositeSettings.Value,
            };
            this.sharedContentRedisInterface = _sharedContentRedisInterface;
          
            status = configuration.GetSection("ContentMode:ContentMode").Get<string>();
            if (this.configuration != null)
            {
                string expiryAppString = this.configuration.GetSection(ExpiryAppSettings).Get<string>();
                this.expiry = double.Parse(string.IsNullOrEmpty(expiryAppString) ? "4" : expiryAppString);
            }
        }

        [HttpGet]
        [Route("/head/[controller]/{id?}")]
        public virtual IActionResult Head()
        {
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/bodytop/[controller]/{id?}")]
        public virtual IActionResult BodyTop()
        {
            
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/breadcrumb/[controller]/{id?}")]
        public virtual IActionResult Breadcrumb()
        {
            var pagesThatDontNeedBreadCrumbs = new List<CompositeViewModel.PageId>
            {
                CompositeViewModel.PageId.Home, CompositeViewModel.PageId.DeleteAccount, CompositeViewModel.PageId.SessionTimeout
            };

            ViewModel.ShowBreadCrumb = !pagesThatDontNeedBreadCrumbs.Contains(ViewModel.Id);
            
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/body/[controller]/{id?}")]
        public virtual async Task<IActionResult> Body()
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "PUBLISHED";
            }

            var sharedhtml = await sharedContentRedisInterface.GetDataAsyncWithExpiry<SharedHtml>(Constants.SpeakToAnAdviserSharedContent, status, expiry);

            ViewModel.SharedSideBar = sharedhtml.Html;


            return View(ViewModel);
        }

        [HttpGet]
        [Route("/bodyfooter/[controller]/{id?}")]
        public virtual IActionResult BodyFooter()
        {
            return View(ViewModel);
        }

        protected IActionResult RedirectTo(string relativeAddress)
        {
            relativeAddress = $"~{ViewModel.CompositeSettings.Path}/" + relativeAddress;
            
            return Redirect(relativeAddress);
       }

        protected async Task<Customer> GetCustomerDetails()
        {
            return await _authService.GetCustomer(User);
        }
    }
}
