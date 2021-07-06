using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DFC.App.Account.Application.Common.Models
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCommon
    {
        public static class RegexPatterns
        {
            public static class PhoneNumber
            {
                public const String ContactPhone = @"^((\(?0\d{4}\)?\s?\d{3}\s?(\d{3}|\d{2}))|(\(?0\d{3}\)?\s?\d{3}\s?(\d{4}|\d{3}))|(\(?0\d{2}\)?\s?\d{4}\s?(\d{4}|\d{3})))(\s?\#(\d{4}|\d{3}))?$";
                public const String ContactMobilePhone = @"^(\+44\s?7\d{3}|\(?07\d{3}\)?)\s?\d{3}\s?(\d{3}|\d{2})$";
            }
            public static class Other
            {
                public const String Name = @"^[A-Za-z'-. `]*$";
                public const String Day = @"^(0[1-9]|[1-9]|[1-2][0-9]|3[0-1])$";
                public const String Month = @"^(0[1-9]|[1-9]|1[0-2])$";
                public const String Numeric = @"^[0-9]*$";
            }

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
