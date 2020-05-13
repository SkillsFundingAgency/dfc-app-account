using DFC.App.Account.Application.Common.Models;
using DFC.App.Account.Models.AddressSearch;
using DFC.App.Account.Services.Interfaces;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.Account.Services
{
    public class AddressSearchService : IAddressSearchService
    {
        private readonly AddressSearchServiceSettings _settings;
        private readonly IRestClient _restClient;
        public AddressSearchService(IOptions<AddressSearchServiceSettings> settings, IRestClient restClient)
        {
            _settings = settings.Value;
            _restClient = restClient;
        }

        public AddressSearchService(IOptions<AddressSearchServiceSettings> settings)
        {
            _settings = settings.Value;
            _restClient = new RestClient();
        }

        private string BuildUrlToFindAddresses(string searchterm)
        {
            return
                string.Concat(
                    _settings.FindAddressesBaseUrl,
                    "&Key=" + System.Web.HttpUtility.UrlEncode(_settings.Key),
                    "&SearchTerm=" + System.Web.HttpUtility.UrlEncode(searchterm),
                    "&LastId=",
                    "&SearchFor=PostalCodes",
                    "&Country=GBR",
                    "&LanguagePreference=EN",
                    "&MaxSuggestions=",
                    "&MaxResults="
                );
        }

        private string BuildUrlToRetrieveAddress(string searchterm)
        {
            return
                string.Concat(
                    _settings.RetrieveAddressBaseUrl,
                    "&Key=" + System.Web.HttpUtility.UrlEncode(_settings.Key),
                    "&Id=" + System.Web.HttpUtility.UrlEncode(searchterm)
                );
        }

        public async Task<IEnumerable<PostalAddressModel>> GetAddresses(string postalCode)
        {
            var url = BuildUrlToFindAddresses(postalCode);

            var result = await _restClient.GetAsync<IEnumerable<PostalAddressModel>>(url);

            return _restClient.LastResponse.IsSuccess ? result : null;
        }

        public async Task<PostalAddressModel> GetAddress(string addressIdentifier)
        {

            var url = BuildUrlToRetrieveAddress(addressIdentifier);
            var result = await _restClient.GetAsync<IEnumerable<PostalAddressModel>>(url);

            return _restClient.LastResponse.IsSuccess ? result.FirstOrDefault() : null;
        }
    }
}
