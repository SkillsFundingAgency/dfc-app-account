using Microsoft.AspNetCore.Mvc;

namespace DFC.App.Account.ViewComponents.Row
{
    public class Row : ViewComponent
    {
        public IViewComponentResult Invoke(RowModel model)
        {
            return View("~/ViewComponents/Row/Default.cshtml", model);
        }
    }
}
