using System.Threading.Tasks;
using DFC.App.Account.Models;
using DFC.App.Account.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.Account.Controllers
{
    public class EditDetailsController : CompositeSessionController<EditDetailsCompositeViewModel>
    {
        public EditDetailsController(IOptions<CompositeSettings> compositeSettings) : base(compositeSettings)
        {
            
        }
        public override async Task<IActionResult> Body()
        {
            return await base.Body();
        }
    }
}