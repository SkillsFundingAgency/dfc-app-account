using Microsoft.AspNetCore.Mvc;

namespace DFC.App.Account.Controllers
{
    public class YourAccountController : Controller
    {
        public IActionResult Body()
        {
            return View();
        }
    }
}