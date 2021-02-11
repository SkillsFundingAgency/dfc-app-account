using DFC.App.Account.Application.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Account.Application.Common.CustomAttributes
{
    // TelephoneNumberAttribute inherits from DoubleRegexAttribute and it's logic is applied in ExtendedDataAnnotationsModelValidatorProvider,
    // where under condition if ContactPreference set to Phone, DoubleRegexAttribute is enforced to be Required and 
    // and the property value is valided against Phone and Mobile Regexes
    [ExcludeFromCodeCoverage]
    public class TelephoneNumberAttribute : PhoneRegexAttribute
    {
        /// <summary>
        /// Gets or Sets the dependent property for this attribute, this is used by the validation method.
        /// </summary>
        public string DependsOn { get; set; }
        public CommonEnums.Channel Type { get; set; }

        public const string DefaultRequireMessage = "You have selected a contact preference which requires a valid phone number";
        public string NonRequiredRegexErrorMessage { get; set; }
        public string BaseErrorMessage { get; set; }

        public TelephoneNumberAttribute() : base()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // We have to get the selection value of the property on which the logic is dependent (ContactPreference)
            var propertyContactPreference = validationContext.ObjectType.GetProperty(this.DependsOn);
            if (propertyContactPreference == null)
            {
                throw new MissingMemberException("Missing member DependsOn - Should be set to contact preference.");
            }

            var contactPref = (CommonEnums.Channel)propertyContactPreference.GetValue(validationContext.ObjectInstance);

            
            if (contactPref == CommonEnums.Channel.Text)
            {
                base.ErrorMessage = DefaultRequireMessage;
                base.IsRequired = true;
            }else if (contactPref == Type)
            {
                base.IsRequired = true;
                base.ErrorMessage = BaseErrorMessage;
            }
            else
            {
                base.ErrorMessage = NonRequiredRegexErrorMessage;
            }

            return base.IsValid(value, validationContext);
        }
    }
}
