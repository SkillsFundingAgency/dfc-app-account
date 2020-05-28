using System;


namespace DFC.App.Account.Application.Common.Extensions
{
    public static class HtmlExtensions
    {

        /// <summary>
        /// Gets the class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="modelState">State of the model.</param>
        /// <returns></returns>
        public static string GetClass(string fieldName, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {

            return modelState.ContainsKey(fieldName) && modelState[fieldName].Errors.Count > 0 ? "govuk-form-group--error" : String.Empty;
                
        }

        /// <summary>
        /// Gets the dob class.
        /// </summary>
        /// <param name="fieldNameDOB">The field name dob.</param>
        /// <param name="fieldNameDOBday">The field name do bday.</param>
        /// <param name="fieldNameDOBmonth">The field name do bmonth.</param>
        /// <param name="fieldNameDOByear">The field name do byear.</param>
        /// <param name="modelState">State of the model.</param>
        /// <returns></returns>
        public static string GetDOBClass(string fieldNameDOB, string fieldNameDOBday, string fieldNameDOBmonth, string fieldNameDOByear, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            bool dobOnError = false;
            bool dobDayOnError = false;
            bool dobMonthOnError = false;
            bool dobYearOnError = false;
            if (modelState.IsValid == false)
            {
                dobOnError = modelState.ContainsKey(fieldNameDOB) && modelState[fieldNameDOB].Errors.Count > 0;
                dobDayOnError = modelState.ContainsKey(fieldNameDOBday) && modelState[fieldNameDOBday].Errors.Count > 0;
                dobMonthOnError = modelState.ContainsKey(fieldNameDOBmonth) && modelState[fieldNameDOBmonth].Errors.Count > 0;
                dobYearOnError = modelState.ContainsKey(fieldNameDOByear) && modelState[fieldNameDOByear].Errors.Count > 0;
            }

            if (!dobOnError && !dobDayOnError && !dobMonthOnError && !dobYearOnError)
            {
                return string.Empty;
            }
            else
            {
                return "govuk-form-group--error";
            }
        }

    }
}
