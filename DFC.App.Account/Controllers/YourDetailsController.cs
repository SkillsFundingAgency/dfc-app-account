using DFC.App.Account.Services.DSS.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.Account.Controllers
{
    public class YourDetailsController : Controller
    {
        public IActionResult Body()
        {
            return View();
        }
    }
}