using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Dfc.ProviderPortal.Packages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DFC.App.Account.Services.DSS.Services
{
    public class DssService : IDssWriter
    {
        private readonly IRestClient _restClient;
        private readonly IOptions<DssSettings> _dssSettings;

        public DssService(IOptions<DssSettings> settings)
        {
            Throw.IfNull(settings, nameof(settings));
            Throw.IfNullOrWhiteSpace(settings.Value.CustomerApiUrl, nameof(settings.Value.CustomerApiUrl));
            Throw.IfNullOrWhiteSpace(settings.Value.ApiKey, nameof(settings.Value.ApiKey));
            Throw.IfNullOrWhiteSpace(settings.Value.AccountsTouchpointId, nameof(settings.Value.AccountsTouchpointId));
            Throw.IfNullOrWhiteSpace(settings.Value.CustomerApiVersion, nameof(settings.Value.CustomerApiVersion));
            _restClient = new RestClient();
            _dssSettings = settings;

        }

        public DssService(IRestClient restClient, IOptions<DssSettings> settings)
        {
            Throw.IfNull(restClient, nameof(restClient));
            Throw.IfNull(settings, nameof(settings));
            Throw.IfNullOrWhiteSpace(settings.Value.CustomerApiUrl, nameof(settings.Value.CustomerApiUrl));
            Throw.IfNullOrWhiteSpace(settings.Value.ApiKey, nameof(settings.Value.ApiKey));
            Throw.IfNullOrWhiteSpace(settings.Value.AccountsTouchpointId, nameof(settings.Value.AccountsTouchpointId));
            Throw.IfNullOrWhiteSpace(settings.Value.CustomerApiVersion, nameof(settings.Value.CustomerApiVersion));
            _restClient = restClient;
            _dssSettings = settings;

        }
        public async Task<Customer> CreateCustomerData(Customer customerData)
        {
            if(customerData == null)
                return null;

            try
            {
                var postData = new StringContent(
                    JsonConvert.SerializeObject(customerData),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json);

                var request = BuildRequestMessage(HttpMethod.Post);
                request.Headers.Add("version", _dssSettings.Value.CustomerApiVersion);
                request.Content = postData;

                var result = await _restClient.PostAsync<Customer>(_dssSettings.Value.CustomerApiUrl, request);
                if (_restClient.LastResponse.IsSuccess)
                {
                    return result;
                }
            }

            catch (Exception e)
            {
                throw new Exception($"CreateCustomerDetail failure. Code: {_restClient.LastResponse.StatusCode}");
            }

            return null;
        }

        public async Task<Customer> UpdateCustomerData(Customer customerData)
        {
            if (customerData == null)
                return null;

            try
            {
                var postData = new StringContent(
                    JsonConvert.SerializeObject(customerData),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json);

                var request = BuildRequestMessage(HttpMethod.Patch);
                request.Headers.Add("version", _dssSettings.Value.CustomerApiVersion);
                request.Content = postData;

                var result = await _restClient.PatchAsync<Customer>(_dssSettings.Value.CustomerApiUrl, request);
                if (_restClient.LastResponse.IsSuccess)
                {
                    return result;
                }
            }

            catch (Exception e)
            {
                throw new Exception($"UpdateCustomerData failure. Code: {_restClient.LastResponse.StatusCode}");
            }

            return null;
        }

        private HttpRequestMessage BuildRequestMessage(HttpMethod method)
        {
            var request = new HttpRequestMessage();
            request.Method = method;
            request.Headers.Add("Ocp-Apim-Subscription-Key", _dssSettings.Value.ApiKey);
            request.Headers.Add("TouchpointId", _dssSettings.Value.AccountsTouchpointId);

            return request;
        }
    }
}
