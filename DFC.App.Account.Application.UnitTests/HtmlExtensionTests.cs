using DFC.App.Account.Application.Common.Extensions;
using NUnit.Framework;

namespace DFC.App.Account.Application.UnitTests
{
    public class HtmlExtensionTests
    {

        private const string DobField = "dob";
        private const string DobDay = "day";
        private const string DobMonth = "month";
        private const string DobYear = "year";
        private const string ErrorClass = "govuk-form-group--error";

        [Test]
        public void When_GetClassCalledForErrorState_ReturnCorrectClass()
        {
            var model = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
            model.AddModelError("test", "error");
            var result  = HtmlExtensions.GetClass("test", model);

            Assert.AreEqual(ErrorClass, result);
        }

        [Test]
        public void When_GetClassCalledForValidState_ReturnCorrectClass()
        {
            var model = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
            var result = HtmlExtensions.GetClass("test", model);

            Assert.IsEmpty(result);
        }
        
        [TestCase(DobField, ErrorClass)]
        [TestCase(DobDay, ErrorClass)]
        [TestCase(DobMonth, ErrorClass)]
        [TestCase(DobYear, ErrorClass)]
        [TestCase("test", "")]
        public void When_DobVariableGetClassCalled_ReturnCorrectClass(string field, string expected)
        {
           

            var model = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
            model.AddModelError(field, "error");
            var result = HtmlExtensions.GetDOBClass(DobField, DobDay, DobMonth, DobYear, model);

            Assert.AreEqual(result, expected);
        }
    }
}
