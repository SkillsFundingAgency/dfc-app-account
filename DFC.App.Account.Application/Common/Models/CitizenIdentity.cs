using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Account.Application.Common.Models
{
    [ExcludeFromCodeCoverage]
    public class CitizenIdentity
    {
        public PersonalDetails PersonalDetails { get; set; }

        public ContactDetails ContactDetails { get; set; }

        public MarketingPreferences MarketingPreferences { get; set; }

    }
}
