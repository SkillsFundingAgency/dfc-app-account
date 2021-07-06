using DFC.App.Account.Application.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.Account.Application.Common.Models
{
    public class PersonalDetails
    {
        [Display(Name = "Title", Order = 1)]
        public CommonEnums.Title Title { get; set; }

        [RegularExpression(ServiceCommon.RegexPatterns.Other.Name, ErrorMessage = "First name contains invalid characters")]
        [StringLength(50, ErrorMessage = "First name too long (max. 50)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter first name")]
        [Display(Name = "First name", Order = 2)]
        public string GivenName { get; set; }

        [RegularExpression(ServiceCommon.RegexPatterns.Other.Name, ErrorMessage = "Last name contains invalid characters")]
        [StringLength(50, ErrorMessage = "Last name too long (max. 50)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter last name")]
        [Display(Name = "Last name", Order = 3)]
        public string FamilyName { get; set; }
        
        [Display(Name = "Date of birth", Order = 4)]
        public Nullable<DateTime> DateOfBirth { get; set; }

        [RegularExpression(ServiceCommon.RegexPatterns.Other.Day, ErrorMessage = "Day must be a number between 1 and 31")]
        [Display(Name = "Day", Order = 5)]
        public string DateOfBirthDay { get; set; }

        [RegularExpression(ServiceCommon.RegexPatterns.Other.Month, ErrorMessage = "Month must be a number between 1 and 12")]
        [Display(Name = "Month", Order = 6)]
        public string DateOfBirthMonth { get; set; }

        [RegularExpression(ServiceCommon.RegexPatterns.Other.Numeric, ErrorMessage = "Year must be a number")]
        [Display(Name = "Year", Order = 7)]
        public string DateOfBirthYear { get; set; }

        [Display(Name = "Gender", Order = 8)]
        public CommonEnums.Gender Gender { get; set; }
    }
}