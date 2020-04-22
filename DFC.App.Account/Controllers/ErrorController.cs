using Microsoft.AspNetCore.Mvc;

namespace DFC.App.Account.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Body()
        {
            return View();
        }
    }
}