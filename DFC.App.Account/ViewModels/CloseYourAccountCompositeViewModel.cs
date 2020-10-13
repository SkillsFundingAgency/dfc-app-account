using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        public string GetErrorClass(string elementName, ModelStateDictionary state)
        {
            var elementState = state[elementName];

            if (elementState != null && elementState.ValidationState == ModelValidationState.Invalid)
            {
                return "govuk-input--error";
            }

            return string.Empty;
        }
    }

}
