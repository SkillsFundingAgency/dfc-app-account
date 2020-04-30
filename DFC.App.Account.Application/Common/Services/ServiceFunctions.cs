using System;
using System.Text.RegularExpressions;

namespace DFC.App.Account.Application.Common.Services
{
    public static class ServiceFunctions
    {
        internal static RegexOptions commonRegexOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline;

        public static bool IsValidRegexValue(string value, string regexPattern)
        {
            return string.IsNullOrWhiteSpace(value) == false &&
                   Regex.IsMatch(value, regexPattern, commonRegexOptions);
        }

        public static Boolean IsValidDoubleRegexValue(string value, string regexPatternOne, string regexPatternTwo, bool isAndOperator)
        {
            if (isAndOperator)
            {
                return string.IsNullOrWhiteSpace(value) == false &&
                       (
                           IsValidRegexValue(value, regexPatternOne) &&
                           IsValidRegexValue(value, regexPatternTwo)
                       );
            }
            else
            {
                return string.IsNullOrWhiteSpace(value) == false &&
                       (
                           IsValidRegexValue(value, regexPatternOne) ||
                           IsValidRegexValue(value, regexPatternTwo)
                       );
            }
        }
    }
}
