using DFC.App.Account.Application.Common.Models;
using DFC.App.Account.Exception;
using DFC.App.Account.Models;
using DFC.App.Account.Models.AddressSearch;
using DFC.App.Account.Services.Interfaces;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.Account.Services
{
    public class GetAddressIoSearchService : IAddressSearchService
    {
        private readonly AddressSearchServiceSettings _settings;
        private readonly IRestClient _restClient;
        public GetAddressIoSearchService(IOptions<AddressSearchServiceSettings> settings, IRestClient restClient)
        {
            _settings = settings.Value;
            _restClient = restClient;
        }

        public GetAddressIoSearchService(IOptions<AddressSearchServiceSettings> settings)
        {
            _settings = settings.Value;
            _restClient = new RestClient();
        }

        public async Task<IEnumerable<PostalAddressModel>> GetAddresses(string postalCode)
        {

            var url = $"{_settings.FindAddressesBaseUrl}/{postalCode}?api-key={_settings.Key}&expand=true";

            var result = await _restClient.GetAsync<GetAddressIoResult>(url);
            if (!_restClient.LastResponse.IsSuccess)
            {
                switch (_restClient.LastResponse.StatusCode)
                {
                    case HttpStatusCode.TooManyRequests:
                        throw new AddressServiceRequestLimitReachedException(_restClient.LastResponse.Content);
                }

                return null;
            }
            
            return result.MapToPostalAddressModel();
        }

        public Task<PostalAddressModel> GetAddress(string addressIdentifier)
        {
            return Task.FromResult<PostalAddressModel>(null);
        }
    }
}
