using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.Account.ViewModels
{
    public class CloseYourAccountCompositeViewModel : CompositeViewModel
    {
        public CloseYourAccountCompositeViewModel() : base(PageId.CloseYourAccount, "Close your account")
        {
           
        }
       
        [Required(ErrorMessage = "Invalid password")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public Guid CustomerId { get; set; }
    }

}
