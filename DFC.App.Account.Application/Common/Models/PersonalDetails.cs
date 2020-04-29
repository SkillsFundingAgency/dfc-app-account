using System;
using System.ComponentModel.DataAnnotations;
using DFC.App.Account.Application.Common.CustomAttributes;
using DFC.App.Account.Application.Common.Enums;

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

        [AgeRange(13, 120,
            MinAgeErrorMessage = "You must be over 13 to use this service",
            MaxAgeErrorMessage = "You must be under 120 to use this service",
            InvalidErrorMessage = "Enter a valid date of birth")]
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

        [HomePostCode(
            UKPostCodeRegex = @"(?ixs)^([bB][fF][pP][oO]\s{0,1}[0-9]{1,4}|[gG][iI][rR]\s{0,1}0[aA][aA]|[a-pr-uwyzA-PR-UWYZ]([0-9]{1,2}|([a-hk-yA-HK-Y][0-9]|[a-hk-yA-HK-Y][0-9]([0-9]|[abehmnprv-yABEHMNPRV-Y]))|[0-9][a-hjkps-uwA-HJKPS-UW])\s{0,1}[0-9][abd-hjlnp-uw-zABD-HJLNP-UW-Z]{2})$",
            EnglishOrBFPOPostCodeRegex = @"(?ixs)^(?!ab|bt|cf|ch5|ch6|ch7|ch8|dd|dg|eh|fk|g[0-9]|gy|hs|im|iv|je|ka|kw|ky|ld|ll|ml|np|pa|ph|sa|sy|td|ze)+.*$",
            BfpoPostCodeRegex = @"(?ixs)^([bB][fF][pP][oO]\s{0,1}[0-9]{1,4})$")]
        [RegularExpression(ServiceCommon.RegexPatterns.PostCode.Postcode, ErrorMessage = "Home postcode contains invalid characters")]
        [StringLength(8, ErrorMessage = "Home postcode too long (max. 8)")]
        [Display(Name = "Enter your postcode<span class=\"form-hint\">Use your postcode to find your address. <br />For example, SW1A 1AA.</span>", Order = 9)]
        public string HomePostCode { get; set; }

        public string FindAddressServiceResult { get; set; }

        [BFPOAddress(DependsOn = "HomePostCode", DependsOnPropertyForAttribute = "HomePostCode",
            IsNonEnglishBfpo = true,
            ErrorMessage = "Enter an alternative postcode")]
        [DoubleRegex(
            FirstRegex = @"(?ixs)^([bB][fF][pP][oO]\s{0,1}[0-9]{1,4}|[gG][iI][rR]\s{0,1}0[aA][aA]|[a-pr-uwyzA-PR-UWYZ]([0-9]{1,2}|([a-hk-yA-HK-Y][0-9]|[a-hk-yA-HK-Y][0-9]([0-9]|[abehmnprv-yABEHMNPRV-Y]))|[0-9][a-hjkps-uwA-HJKPS-UW])\s{0,1}[0-9][abd-hjlnp-uw-zABD-HJLNP-UW-Z]{2})$",
            SecondRegex = @"(?ixs)^(?!ab|bt|cf|ch5|ch6|ch7|ch8|dd|dg|eh|fk|g[0-9]|gy|hs|im|iv|je|ka|kw|ky|ld|ll|ml|np|pa|ph|sa|sy|td|ze)+.*$",
            IsAndOperator = true,
            IsRequired = false,
            ErrorMessage = "Alternative postcode must be an English or BFPO postcode")]
        [RegularExpression(ServiceCommon.RegexPatterns.PostCode.Postcode, ErrorMessage = "Alternative postcode contains invalid characters")]
        [StringLength(8, ErrorMessage = "Alternative postcode too long (max. 8)")]
        [Display(Name = "Alternative postcode <span class=\"form-hint\">If your address is not in England, provide an alternative postcode.</span>", Order = 10)]
        public string AlternativePostCode { get; set; }

        [ConditionalRequired(DependsOn = "HomePostCode", ErrorMessage = "First line of your address is required")]
        [RegularExpression(ServiceCommon.RegexPatterns.Other.AddressString, ErrorMessage = "First line of your address contains invalid characters")]
        [StringLength(80, ErrorMessage = "First line of your address is too long (max. 80)")]
        [Display(Name = "First line of your address", Order = 11)]
        public string AddressLine1 { get; set; }

        [BFPOAddress(DependsOn = "HomePostCode", DependsOnPropertyForAttribute = "HomePostCode",
            ErrorMessage = "Second line of your address is required")]
        [RegularExpression(ServiceCommon.RegexPatterns.Other.AddressString, ErrorMessage = "Second line of your address contains invalid characters")]
        [StringLength(80, ErrorMessage = "Second line of your address is too long (max. 80)")]
        [Display(Name = "Second line of your address", Order = 12)]
        public string AddressLine2 { get; set; }

        [BFPOAddress(DependsOn = "HomePostCode", DependsOnPropertyForAttribute = "HomePostCode",
            ErrorMessage = "Third line of your address is required")]
        [RegularExpression(ServiceCommon.RegexPatterns.Other.AddressString, ErrorMessage = "Third line of your address contains invalid characters")]
        [StringLength(80, ErrorMessage = "Third line of your address is too long (max. 80)")]
        [Display(Name = "Third line of your address", Order = 13)]
        public string AddressLine3 { get; set; }

        [RegularExpression(ServiceCommon.RegexPatterns.Other.AddressString, ErrorMessage = "Fourth line of your address contains invalid characters")]
        [StringLength(80, ErrorMessage = "Fourth line of your address is too long (max. 80)")]
        [Display(Name = "Fourth line of your address", Order = 14)]
        public string AddressLine4 { get; set; }

        [RegularExpression(ServiceCommon.RegexPatterns.Other.AddressString, ErrorMessage = "Fifth line of your address contains invalid characters")]
        [StringLength(80, ErrorMessage = "Fifth line of your address is too long (max. 80)")]
        [Display(Name = "Fifth line of your address", Order = 15)]
        public string AddressLine5 { get; set; }

        [BFPOAddress(DependsOn = "HomePostCode", DependsOnPropertyForAttribute = "HomePostCode",
            IsNotBfpo = true,
            ErrorMessage = "Town is required")]
        [RegularExpression(ServiceCommon.RegexPatterns.Other.AddressString, ErrorMessage = "Town contains invalid characters")]
        [StringLength(80, ErrorMessage = "Town is too long (max. 80)")]
        [Display(Name = "Town", Order = 16)]
        public string Town { get; set; }


    }
}