using Microsoft.AspNetCore.Mvc;

namespace DFC.App.Account.Controllers
{
    public class SessionTimeoutController : Controller
    {
        public IActionResult Body()
        {
            return View();
        }
    }
}