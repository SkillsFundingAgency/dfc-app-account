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
        private const string CustomerUrl = "/customers/api/Customers/";

        public DssService(IOptions<DssSettings> settings)
        {
            Throw.IfNull(settings, nameof(settings));
            Throw.IfNullOrWhiteSpace(settings.Value.ApiUrl, nameof(settings.Value.ApiUrl));
            Throw.IfNullOrWhiteSpace(settings.Value.ApiKey, nameof(settings.Value.ApiKey));
            Throw.IfNullOrWhiteSpace(settings.Value.AccountsTouchpointId, nameof(settings.Value.AccountsTouchpointId));
            Throw.IfNullOrWhiteSpace(settings.Value.Version, nameof(settings.Value.Version));
            _restClient = new RestClient();
            _dssSettings = settings;

        }

        public DssService(IRestClient restClient, IOptions<DssSettings> settings)
        {
            Throw.IfNull(restClient, nameof(restClient));
            Throw.IfNull(settings, nameof(settings));
            Throw.IfNullOrWhiteSpace(settings.Value.ApiUrl, nameof(settings.Value.ApiUrl));
            Throw.IfNullOrWhiteSpace(settings.Value.ApiKey, nameof(settings.Value.ApiKey));
            Throw.IfNullOrWhiteSpace(settings.Value.AccountsTouchpointId, nameof(settings.Value.AccountsTouchpointId));
            Throw.IfNullOrWhiteSpace(settings.Value.Version, nameof(settings.Value.Version));
            _restClient = restClient;
            _dssSettings = settings;

        }
        public async Task<Customer> CreateCustomerData(Customer customerData)
        {
            if(customerData == null)
                return null;

            var postData = new StringContent(
                JsonConvert.SerializeObject(customerData),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            postData = AddRequestHeaders(postData);
            
            var result = await _restClient.PostAsync<Customer>($"{_dssSettings.Value.ApiUrl}{CustomerUrl}", postData, _dssSettings.Value.ApiKey);
            if (_restClient.LastResponse.IsSuccess)
            {
                return result;
            }

            return null;

        }
        private StringContent AddRequestHeaders(StringContent content)
        {
            content.Headers.Add("TouchpointId", _dssSettings.Value.AccountsTouchpointId);
            content.Headers.Add("version", _dssSettings.Value.Version);
            content.Headers.Add("Ocp-Apim-Subscription-Key", _dssSettings.Value.ApiKey);
            return content;
        }
    }
}
