using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.ViewModels;
using DFC.APP.Account.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DFC.App.Account.Controllers
{

    /// <summary>
    /// Adds default Composite UI endpoints and routing logic to the base session controller.
    /// </summary>
    public abstract class CompositeSessionController<TViewModel>:Controller where TViewModel : CompositeViewModel, new()
    {
        private readonly IAuthService _authService;
        private readonly IDocumentService<CmsApiSharedContentModel> _documentService;
        private readonly Guid _sharedContent;

        protected TViewModel ViewModel { get; }
        protected CompositeSessionController(IOptions<CompositeSettings> compositeSettings, IAuthService authService, IDocumentService<CmsApiSharedContentModel> documentService, IConfiguration config)
        {
            _authService = authService;
            ViewModel = new TViewModel()
            {
                CompositeSettings = compositeSettings.Value,
            };
            _sharedContent = config.GetValue<Guid>("SharedContentGuid");
            _documentService = documentService;
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
            var sharedContent = await _documentService.GetByIdAsync(_sharedContent,"account").ConfigureAwait(false);
            ViewModel.SharedSideBar = sharedContent?.Content;
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
