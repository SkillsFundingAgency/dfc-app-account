using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.Account.Models;
using DFC.App.Account.ViewModels;
using Microsoft.Extensions.Logging;

namespace DFC.App.Account.Controllers
{
   
    /// <summary>
    /// Adds default Composite UI endpoints and routing logic to the base session controller.
    /// </summary>
    public abstract class CompositeSessionController<TViewModel>:Controller where TViewModel : CompositeViewModel, new()
    {
        protected TViewModel ViewModel { get; }
        protected CompositeSessionController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings)
        {
            ViewModel = new TViewModel()
            {
                CompositeSettings = compositeSettings.Value,
            };  
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
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/body/[controller]/{id?}")]
        public virtual Task<IActionResult> Body()
        {
            return Task.FromResult<IActionResult>(View(ViewModel));
        }

        [HttpGet]
        [Route("/bodyfooter/[controller]/{id?}")]
        public virtual IActionResult BodyFooter()
        {
            return View(ViewModel);
        }
        protected virtual IActionResult RedirectWithError(string controller, string parameters = "")
        {

            if (!string.IsNullOrEmpty(parameters))
            {
                 parameters = $"&{parameters}";
            }

            return RedirectTo($"{controller}?errors=true{parameters}");
        }

       
        protected bool HasErrors()
        {
            var errorsString = Request.Query["errors"];
            var parsed = bool.TryParse(errorsString, out var error);
            return parsed && error;
        }

        protected IActionResult RedirectTo(string relativeAddress)
        {
            relativeAddress = $"~{ViewModel.CompositeSettings.Path}/" + relativeAddress;
            
            return Redirect(relativeAddress);
       }
    }
}
