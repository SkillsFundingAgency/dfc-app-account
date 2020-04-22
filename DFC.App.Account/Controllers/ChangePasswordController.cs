using Microsoft.AspNetCore.Mvc;

namespace DFC.App.Account.Controllers
{
    public class ChangePasswordController : Controller
    {
        public IActionResult Body()
        {
            return View();
        }
    }
}