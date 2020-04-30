using System.ComponentModel.DataAnnotations;

namespace DFC.App.Account.ViewModels
{
    public class CloseYourAccountCompositeViewModel : CompositeViewModel
    {
        public CloseYourAccountCompositeViewModel() : base(PageId.CloseYourAccount, "Close Your Account")
        {
        }
        [StringLength(250, MinimumLength = 1, ErrorMessage = "You may only have up to 250 characters")]
        [Required(ErrorMessage = "Enter a reason for closing your account")]
        [Display(Name = "Tell us why you want to close your account")]
        public string Reason { get; set; }

        [Required(ErrorMessage = "Invalid password")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public bool HasError { get; set; }
    }
}
