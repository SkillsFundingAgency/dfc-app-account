using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Account.Application.Common.Models
{
    [ExcludeFromCodeCoverage]
    public class MarketingPreferences
    {
        public bool OptOutOfMarketing { get; set; }
        public bool MarketingOptIn
        {
            get
            {
                return !OptOutOfMarketing;
            }
            set
            {
                OptOutOfMarketing = !value;
            }
        }

        public bool OptOutOfMarketResearch { get; set; }
        public bool MarketResearchOptIn
        {
            get
            {
                return !OptOutOfMarketResearch;
            }
            set
            {
                OptOutOfMarketResearch = !value;
            }
        }
    }
}
