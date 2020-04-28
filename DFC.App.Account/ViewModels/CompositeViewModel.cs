using DFC.App.Account.Models;
using DFC.Personalisation.Common.Extensions;
using Dfc.ProviderPortal.Packages;

namespace DFC.App.Account.ViewModels
{
    public abstract class CompositeViewModel
    {
        public static string AppTitle => "Your Account";

        public class PageId
        {
            private PageId(string value)
            {
                Throw.IfNullOrWhiteSpace(value, nameof(value));
                Value = value.Trim();
            }

            public override string ToString()
            {
                return Value;
            }

            public string Value { get; }

            public static PageId Home { get; } = new PageId("home");
            public static PageId Error { get; } = new PageId("error");
            public static PageId ChangePassword { get; } = new PageId("change-password");
            public static PageId CloseYourAccount { get; } = new PageId("close-your-account");
            public static PageId YourDetails { get; } = new PageId("your-details");
            public static PageId EditDetails { get; } = new PageId("edit-details");
            public static PageId SessionTimeout { get; } = new PageId("sessionTimeout");
        }

        public class PageRegion
        {
            private PageRegion(string value)
            {
                Value = value;
            }

            public override string ToString()
            {
                return Value;
            }

            public string Value { get; }
            public static PageRegion Body { get; } = new PageRegion("body");
        }

        public PageId Id { get; }

        public string PageTitle { get; }
        public string PageHeading { get; }

        public CompositeSettings CompositeSettings { get; set; }

        protected CompositeViewModel(PageId pageId, string pageHeading)
        {
            Id = pageId;
            PageHeading = pageHeading;
            PageTitle = string.IsNullOrWhiteSpace(pageHeading) ? AppTitle : $"{pageHeading} | {AppTitle}";
        }

        #region Helpers

        public string GetElementId(string elementName, string instanceName)
        {
            Throw.IfNullOrWhiteSpace(elementName, nameof(elementName));
            Throw.IfNullOrWhiteSpace(instanceName, nameof(instanceName));
            elementName = elementName.FirstCharToUpper().Trim();
            instanceName = instanceName.FirstCharToUpper().Trim();
            return $"{Id}{elementName}{instanceName}";
        }

        public string NounForNumber(int number, string singularNoun, string pluralNoun)
        {
            if (1 == number)
            {
                return singularNoun;
            }
            else
            {
                return pluralNoun;
            }
        }

        #endregion Helpers
    }
}