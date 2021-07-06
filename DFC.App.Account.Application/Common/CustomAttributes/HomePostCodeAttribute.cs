using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Account.Application.Common.CustomAttributes
{
    // HomePostCodeAttribute is not Validation attribute and is used only for storage of the three regexes
    [ExcludeFromCodeCoverage]
    public class HomePostCodeAttribute : Attribute
    {
        public string UKPostCodeRegex { get; set; }
        public string EnglishOrBFPOPostCodeRegex { get; set; }
        public string BfpoPostCodeRegex { get; set; }

        public HomePostCodeAttribute() { }

        public HomePostCodeAttribute(string ukPostCodeRegex, string englishOrBFPOPostCodeRegex, string bfpoPostCodeRegex)
        {
            UKPostCodeRegex = ukPostCodeRegex;
            EnglishOrBFPOPostCodeRegex = englishOrBFPOPostCodeRegex;
            BfpoPostCodeRegex = bfpoPostCodeRegex;
        }
    }
}
