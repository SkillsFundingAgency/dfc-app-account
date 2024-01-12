using DFC.App.Account.Models;
using DFC.App.Account.Services;
using DFC.App.Account.Services.DSS.Models;
using DFC.App.Account.ViewModels;
using DFC.APP.Account.Data.Models;
//using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using DFC.APP.Account.Data.Common;
using Microsoft.Extensions.Configuration;
using DFC.Common.SharedContent.Pkg.Netcore;

namespace DFC.App.Account.Controllers
{

    /// <summary>
    /// Adds default Composite UI endpoints and routing logic to the base session controller.
    /// </summary>
    public abstract class CompositeSessionController<TViewModel>:Controller where TViewModel : CompositeViewModel, new()
    {
        private readonly IAuthService _authService;
        //private readonly IDocumentService<CmsApiSharedContentModel> _documentService;
        private readonly Guid _sharedContent;
        public const string SharedContentStaxId = "2c9da1b3-3529-4834-afc9-9cd741e59788";

        protected TViewModel ViewModel { get; }
        protected CompositeSessionController(IOptions<CompositeSettings> compositeSettings, IAuthService authService, IConfiguration config)
        {
            _authService = authService;
            ViewModel = new TViewModel()
            {
                CompositeSettings = compositeSettings.Value,
            };
            _sharedContent = config.GetValue<Guid>(Constants.SharedContentGuidConfig);
            //_documentService = documentService;
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
            //var sharedContent = await _documentService.GetByIdAsync(_sharedContent,"account").ConfigureAwait(false);
            //ViewModel.SharedSideBar = sharedContent?.Content;


            var cacheKey = "sharedcontent-" + SharedContentStaxId;
            GraphQlActions graphQl = new GraphQlActions();
            List<string> parameters = new List<string>() { cacheKey,"account" };
            var sharedContent = await graphQl.GetDataAsync("shared-html", parameters).ConfigureAwait(false);





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
