using DFC.App.Account.Application.Common.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace DFC.App.Account.ViewModels
{
    public class EditDetailsCompositeViewModel : CompositeViewModel
    {
        public EditDetailsCompositeViewModel() : base(PageId.EditDetails, "Edit Details")
        {
            
        }
        public CitizenIdentity Identity { get; set; }

        public IList<PostalAddressModel> Items { get; set; }

        public IList<PostalAddressModel> SelectedAddress { get; set; }
        public string GetErrorClass(string elementName, ModelStateDictionary state)
        {
            var elementState = state[elementName];

            if (elementState != null && elementState.ValidationState == ModelValidationState.Invalid)
            {
                return "govuk-input--error";
            }

            return string.Empty;
        }

        public string GetFormGroupErrorClass(string elementName, ModelStateDictionary state)
        {
            return string.IsNullOrEmpty(GetErrorClass(elementName, state)) ? "" : "govuk-form-group--error";
        }
    }
}
