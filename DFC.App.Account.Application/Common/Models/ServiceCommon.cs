using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DFC.App.Account.Application.Common.Models
{
    public static class ServiceCommon
    {
        public static class RegexPatterns
        {
            internal static RegexOptions commonRegexOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant;
            public static class PostCode
            {
                public const String Postcode = @"^[A-Za-z0-9-.\(\)\/\\\s]*$";
                public const String UKPostCode = @"^([bB][fF][pP][oO]\s{0,1}[0-9]{1,4}|[gG][iI][rR]\s{0,1}0[aA][aA]|[a-pr-uwyzA-PR-UWYZ]([0-9]{1,2}|([a-hk-yA-HK-Y][0-9]|[a-hk-yA-HK-Y][0-9]([0-9]|[abehmnprv-yABEHMNPRV-Y]))|[0-9][a-hjkps-uwA-HJKPS-UW])\s{0,1}[0-9][abd-hjlnp-uw-zABD-HJLNP-UW-Z]{2})$";
                public const String EnglishOrBFPOPostCode = @"^(?!ab|bt|cf|ch5|ch6|ch7|ch8|dd|dg|eh|fk|g[0-9]|gy|hs|im|iv|je|ka|kw|ky|ld|ll|ml|np|pa|ph|sa|sy|td|ze)+.*$";
                public const String BfpoPostCode = @"^([bB][fF][pP][oO]\s{0,1}[0-9]{1,4})$";
            }
            public static class PhoneNumber
            {
                public const String ContactPhone = @"^[+0]{1}[0-9( )-]{10,19}$";
                public const String ContactMobilePhone = @"^((\+44)|(0044)|(0))7([1-5]|[7-9])[0-9]{8}$";
            }
            public static class Other
            {
                public const String Name = @"^[A-Za-z'-. `]*$";
                public const String Password = @"^(?i:(?!.*pass.*word|.*\s))(?!^.*\n)(?=.*\W*)(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,16}$";
                public const String Email = @"^[A-Za-z0-9_\-\+/{\t\n\r}#$%^\\&\[\]*=()|?'~`! :]+([.][A-Za-z0-9_\-\+/{\t\n\r}#$%^\\&\[\]*=()|?'~`! :]+)*@[A-Za-z0-9\[\]:]+([.-][A-Za-z0-9\[\]:]+)*\.[A-Za-z0-9\[\]:]+([-.][A-Za-z0-9\[\]:]+)*$";
                public const String AddressString = @"^[A-Za-z0-9'-. `_,:&*#\""\(\)\/\\]*$";
                public const String Day = @"^(0[1-9]|[1-9]|[1-2][0-9]|3[0-1])$";
                public const String Month = @"^(0[1-9]|[1-9]|1[0-2])$";
                public const String Numeric = @"^[0-9]*$";
            }

            /// <summary>
            /// Course directory Regex patterns
            /// </summary>
            public static class CourseDirectory
            {
                /// <summary>
                /// The no invalid characters
                /// </summary>
                public const string NoInvalidCharacters = @"^[a-zA-Z0-9& \(\)\+:'’,\./]*$";
            }

            /// <summary>
            /// Job Profile Regex patterns
            /// </summary>
            public static class JobProfile
            {
                /// <summary>
                /// The no invalid characters
                /// </summary>
                public const string NoInvalidCharacters = @"^[a-zA-Z0-9& \(\)\+:'’,\./]*$";
            }
        }

        public static Boolean IsValidEmailAddress(String emailAddress)
        {
            return
                String.IsNullOrWhiteSpace(emailAddress) == false &&
                (Regex.IsMatch(emailAddress, RegexPatterns.Other.Email, RegexPatterns.commonRegexOptions));
        }
        public static Boolean IsValidUkPostcode(String postalCode)
        {
            return
                String.IsNullOrWhiteSpace(postalCode) == false &&
                (Regex.IsMatch(postalCode, RegexPatterns.PostCode.UKPostCode, RegexPatterns.commonRegexOptions));
        }
        public static Boolean IsValidBfpoPostcode(String postalCode)
        {
            return
                String.IsNullOrWhiteSpace(postalCode) == false &&
                (
                    Regex.IsMatch(postalCode, RegexPatterns.PostCode.UKPostCode, RegexPatterns.commonRegexOptions) &&
                    Regex.IsMatch(postalCode, RegexPatterns.PostCode.BfpoPostCode, RegexPatterns.commonRegexOptions)
                );
        }
        public static Boolean IsValidUKEnglandOrBfpoPostcode(String postalCode)
        {
            return
                String.IsNullOrWhiteSpace(postalCode) == false &&
                (
                    Regex.IsMatch(postalCode, RegexPatterns.PostCode.UKPostCode, RegexPatterns.commonRegexOptions) &&
                    Regex.IsMatch(postalCode, RegexPatterns.PostCode.EnglishOrBFPOPostCode, RegexPatterns.commonRegexOptions)
                );
        }
        public static Boolean IsValidContactPhoneOrMobileNumber(String contactNumber)
        {
            return String.IsNullOrWhiteSpace(contactNumber) == false &&
                (
                    Regex.IsMatch(contactNumber, RegexPatterns.PhoneNumber.ContactPhone, RegexPatterns.commonRegexOptions) ||
                    Regex.IsMatch(contactNumber, RegexPatterns.PhoneNumber.ContactMobilePhone, RegexPatterns.commonRegexOptions)
                );
        }
        public static Boolean IsValidContactPhoneNumber(String contactNumber)
        {
            return String.IsNullOrWhiteSpace(contactNumber) == false &&
                Regex.IsMatch(contactNumber, RegexPatterns.PhoneNumber.ContactPhone, RegexPatterns.commonRegexOptions);
        }
        public static Boolean IsValidContactMobilePhoneNumber(String contactNumber)
        {
            return String.IsNullOrWhiteSpace(contactNumber) == false &&
                Regex.IsMatch(contactNumber, RegexPatterns.PhoneNumber.ContactMobilePhone, RegexPatterns.commonRegexOptions);
        }
        public static Boolean ActionPlanActionStatusSelectorIsEnabled(String currentStatus)
        {
            if (String.IsNullOrWhiteSpace(currentStatus) == false &&
                (
                    currentStatus.Equals("", StringComparison.OrdinalIgnoreCase) ||
                    currentStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase)
                )
              )
            {
                return false; //@"disabled=""disabled""";
            }
            return true;
        }

        public static IDictionary<String, String> GetActionPlanStatusOptions(String currentStatus)
        {
            if (String.IsNullOrWhiteSpace(currentStatus) == true)
            {
                return ACTION_PLAN_ACTION_STATUSES;
            }
            else
            {
                return ACTION_PLAN_ACTION_STATUSES.Where((x, i) => i >= ACTION_PLAN_ACTION_STATUSES.Values.ToList().IndexOf(currentStatus)).ToDictionary(x => x.Key, x => x.Value);
            }
        }

        private static readonly IDictionary<String, String> ACTION_PLAN_ACTION_STATUSES =
            new Dictionary<String, String>() {
                { "", "" },
                { "Not Applicable", "NoLongerApplicable" },
                { "Not Started", "PendingNotYetStarted" },
                { "In Progress", "InProgress" },
                { "Completed", "Completed" }
            };

        //this can be moved to configuration
        public static readonly string SERVICE_NAME = "san.id";
        //this can be moved to configuration - there is no web service call I am aware of, which can return following information.
     

        public static IEnumerable<dynamic> JsonArrayToExpando(String item)
        {
            var converter = new ExpandoObjectConverter();
            return JsonConvert.DeserializeObject<ExpandoObject[]>(item, converter) as ExpandoObject[];
        }

        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> anonymousDictionary = anonymousObject.AnonymousObjectToHtmlAttributes();
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
            {
                expando.Add(item);
            }
            return (ExpandoObject)expando;
        }

        internal static RouteValueDictionary AnonymousObjectToHtmlAttributes(this object htmlAttributes)
        {
            RouteValueDictionary routeValueDictionaries = new RouteValueDictionary();
            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    routeValueDictionaries.Add(property.Name.Replace('\u005F', '-'), property.GetValue(htmlAttributes));
                }
            }
            return routeValueDictionaries;
        }

        public static string GetDisplayName(this Enum value)
        {
            try
            {
                DisplayAttribute customAttribute = value.GetType().GetMember(value.ToString()).FirstOrDefault().GetCustomAttribute<DisplayAttribute>();
                return customAttribute != null ? customAttribute.Name : value.ToString();
            }
            catch
            {
                return value.ToString();
            }
        }
    }
}
