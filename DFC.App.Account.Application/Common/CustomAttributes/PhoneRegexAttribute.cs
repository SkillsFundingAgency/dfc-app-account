using DFC.App.Account.Application.Common.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace DFC.App.Account.Application.Common.CustomAttributes
{
    [ExcludeFromCodeCoverage]
    public class PhoneRegexAttribute : ValidationAttribute, IClientValidatable
    {
        // DoubleRegexAttribute is generic validation against two regexes ...
        public string Regex { get; set; }
        public bool IsAndOperator { get; set; }
        // It also can be applied if Required
        public bool IsRequired { get; set; }

        public PhoneRegexAttribute(string firstRegex, string secondRegex, bool isAndOperator, bool isRequired)
        {
            Regex = firstRegex.Trim();
            IsAndOperator = isAndOperator;
            IsRequired = isRequired;
        }

        public PhoneRegexAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            string objectValue = Convert.ToString(value);            
            return (!IsRequired && string.IsNullOrEmpty(objectValue)) || ServiceFunctions.IsValidRegexValue(objectValue, Regex);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = String.IsNullOrEmpty(ErrorMessage) ? FormatErrorMessage(metadata.DisplayName) : ErrorMessage,
                ValidationType = "phoneregex",
            };

            rule.ValidationParameters.Add("regex", Regex);
            rule.ValidationParameters.Add("drerrormessage", ErrorMessage);
            rule.ValidationParameters.Add("isandoperator", IsAndOperator.ToString());
            rule.ValidationParameters.Add("isdrrequired", IsRequired.ToString());
            yield return rule;
        }
    }
}
