using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Dfc.ProviderPortal.Packages;
using Newtonsoft.Json;

namespace DFC.App.Account.Services.DSS.Services
{
    public class DssService : IDssWriter
    {
        private readonly IRestClient _restClient;
        private readonly DssSettings _dssSettings;
        private const string CustomerUrl = "";
        public DssService(IRestClient restClient, DssSettings settings)
        {
            Throw.IfNull(restClient, nameof(restClient));
            Throw.IfNull(settings, nameof(settings));
            Throw.IfNullOrWhiteSpace(settings.ApiUrl, nameof(settings.ApiUrl));
            Throw.IfNullOrWhiteSpace(settings.ApiKey, nameof(settings.ApiKey));
            _restClient = restClient;
            _dssSettings = settings;

        }
        public async Task<HttpResponseMessage> CreateCustomerData(Customer customerdata)
        {
            if(customerdata == null)
                return null;

            var postData = new StringContent(
                JsonConvert.SerializeObject(customerdata),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            var result = await _restClient.PostAsync<HttpResponseMessage>($"{_dssSettings.ApiUrl}{CustomerUrl}", postData, _dssSettings.ApiKey); 

            return result;
        }

    }
}
