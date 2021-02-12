using DFC.App.Account.Application.Common.CustomAttributes;
using DFC.App.Account.Application.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.Account.Application.Common.Models
{
    public class ContactDetails
    {
        [StringLength(100, ErrorMessage = "Email address is too long (max. 100)")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
       // [Required(ErrorMessage = "Enter email address")]
        [Display(Name = "Email address", Order = 17)]
        public string ContactEmail { get; set; }

        [TelephoneNumber(
            DependsOn = "ContactPreference",
            Regex = ServiceCommon.RegexPatterns.PhoneNumber.ContactPhone,
            Type = CommonEnums.Channel.Phone,
            IsAndOperator = false,
            BaseErrorMessage = "You have selected a contact preference which requires a valid telephone number",
            NonRequiredRegexErrorMessage = "Enter a valid telephone number")]
        [Display(Name = "Home number", Order = 18)]
        public string HomeNumber { get; set; }

        [TelephoneNumber(
            DependsOn = "ContactPreference",
            Regex = ServiceCommon.RegexPatterns.PhoneNumber.ContactMobilePhone,
            Type = CommonEnums.Channel.Mobile,
            IsAndOperator = false,
            BaseErrorMessage = "You have selected a contact preference which requires a valid mobile number",
            NonRequiredRegexErrorMessage = "Enter a valid mobile number")]
        [Display(Name = "Mobile number", Order = 18)]
        public string MobileNumber { get; set; }

        [TelephoneNumber(
            DependsOn = "ContactPreference",
            Regex = ServiceCommon.RegexPatterns.PhoneNumber.ContactPhone,
            IsAndOperator = false,
            BaseErrorMessage = "You have selected a contact preference which requires a valid telephone number",
            NonRequiredRegexErrorMessage = "Enter a valid telephone number")]
        [Display(Name = "Alternative phone number", Order = 19)]
        public string TelephoneNumberAlternative { get; set; }

        [Display(Name = "Preferred contact method", Order = 20)]
        public CommonEnums.Channel ContactPreference { get; set; }
    }
}
