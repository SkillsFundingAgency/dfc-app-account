using System.Threading.Tasks;
using DFC.App.Account.ViewComponents.Row;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.Account.ViewComponents.ShowRow
{
    public class Row : ViewComponent
    {
        public IViewComponentResult Invoke(RowModel model)
        {
            return View("~/ViewComponents/Row/Default.cshtml", model);
        }
    }
}
