using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using ExpressionHelper = System.Web.Mvc.ExpressionHelper;
using HtmlHelper = Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper;

namespace DFC.App.Account.Application.Common.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Labels the with hint for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString LabelWithHintFor<TModel, TValue>(this Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);

            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, html.ViewData, html.MetadataProvider);
            var tagText = metadata.Metadata.DisplayName;

            if (string.IsNullOrEmpty(tagText))
            {
                return new MvcHtmlString(string.Empty);
            }

            TagBuilder tag = new TagBuilder("label");

            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            // If forId is supplied we override 'for' attribute value 
            if (attributes.ContainsKey("forId"))
            {
                object idValue;
                attributes.TryGetValue("forId", out idValue);
                string idStrValue = idValue as string;
                fieldId = idStrValue;
            }

            tag.Attributes.Add("for", fieldId);

            if (attributes.ContainsKey("class"))
            {
                object value;
                attributes.TryGetValue("class", out value);
                string classValue = value as string;
                tag.Attributes.Add("class", classValue);
            }

            tag.SetInnerText(tagText);

            return new MvcHtmlString(HttpUtility.HtmlDecode(tag.ToString(TagRenderMode.Normal)));
        }

        /// <summary>
        /// Labels the with hint for using hidden field.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString LabelWithHintForUsingHiddenField<TModel, TValue>(this Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, html.ViewData, html.MetadataProvider);
            var tagText = metadata.Metadata.DisplayName;
            if (string.IsNullOrEmpty(tagText))
            {
                return new MvcHtmlString(string.Empty);
            }

            TagBuilder tag = new TagBuilder("label");

            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            if (attributes.ContainsKey("class"))
            {
                object value;
                attributes.TryGetValue("class", out value);
                string classValue = value as string;
                tag.Attributes.Add("class", classValue);
            }

            tag.SetInnerText(tagText);

            return new MvcHtmlString(HttpUtility.HtmlDecode(tag.ToString(TagRenderMode.Normal)));
        }

        /// <summary>
        /// Gets the class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="modelState">State of the model.</param>
        /// <returns></returns>
        public static string GetClass(string fieldName, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {

            return modelState.ContainsKey(fieldName) && modelState[fieldName].Errors.Count > 0 ? "error" : String.Empty;
                
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
                return "error";
            }
        }

    }
}
