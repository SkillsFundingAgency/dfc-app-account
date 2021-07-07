using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace DFC.App.Account.Application.Common.CustomAttributes
{
    public class AcceptAttribute : ValidationAttribute, IClientValidatable
    {
        [ExcludeFromCodeCoverage]
        public AcceptAttribute()
        {

        }

        public override bool IsValid(object value)
        {
            // AcceptAttribute is applied only to Boolean Properties and allow only values set to true
            if (value == null) return false;
            if (value.GetType() != typeof(bool)) throw new InvalidOperationException("Can only be used on boolean properties.");
            return (bool)value == true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = String.IsNullOrEmpty(ErrorMessage) ? FormatErrorMessage(metadata.DisplayName) : ErrorMessage,
                ValidationType = "enforcetrue"
            };
        }
    }
}
