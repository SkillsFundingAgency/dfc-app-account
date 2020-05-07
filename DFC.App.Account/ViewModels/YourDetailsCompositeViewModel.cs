using DFC.App.Account.Services.DSS.Models;

namespace DFC.App.Account.ViewModels
{
    public class YourDetailsCompositeViewModel : CompositeViewModel
    {
        public YourDetailsCompositeViewModel() : base(PageId.YourDetails, "Your details")
        {
            CustomerDetails = new Customer();
        }
        public Customer CustomerDetails { get; set; }

    }
}
