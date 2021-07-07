using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Account.Models.AddressSearch
{
    [ExcludeFromCodeCoverage]
    public class AddressSearchServiceSettings
    {
        public string FindAddressesBaseUrl { get; set; }
        public string RetrieveAddressBaseUrl { get; set; }
        public string Key { get; set; }
        public string AddressIdentifierPattern { get; set; }
    }
}
