namespace DFC.App.Account.ViewModels
{
    public class HomeCompositeViewModel : CompositeViewModel
    {
        public bool HasErrors { get; set; }
        public HomeCompositeViewModel()
            : base(PageId.Home, "Home")
        {
        }
    }
}