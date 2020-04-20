using System;
using System.Net;
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
    public class DssService : IDssWriter, IDssReader
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

            
            var request = await BuidRequestMessage();
            request.Headers.Add("version", _dssSettings.Value.CustomerApiVersion);

            var result = await _restClient.PostAsync<Customer>(_dssSettings.Value.CustomerApiUrl, request);
            if (_restClient.LastResponse.IsSuccess)
            {
                return result;
            }

            return null;

        }

        public async Task<Customer> GetCustomerData(string customerId)
        {
            
            var request = await BuidRequestMessage();
            request.Headers.Add("version", _dssSettings.Value.CustomerApiVersion);

            var customer = new Customer();
            
            customer = await GetCustomerDetail(customerId, request);
            customer.Addresses = await GetCustomerAddressDetails(customerId, request);
            customer.Contact = await GetCustomerContactDetails(customerId, request);

            return customer;

        }

        private async Task<HttpRequestMessage> BuidRequestMessage()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("Ocp-Apim-Subscription-Key", _dssSettings.Value.ApiKey);
            request.Headers.Add("TouchpointId", _dssSettings.Value.AccountsTouchpointId);
            
            return request;
        }

        public async Task<Customer> GetCustomerDetail(string customerId, HttpRequestMessage request)
        {
            try
            {
                request.Headers.Add("version", _dssSettings.Value.CustomerApiVersion);
                return await  _restClient.GetAsync<Customer>($"{_dssSettings.Value.CustomerApiUrl}{customerId}", request);
            }
            catch (Exception e)
            {
                throw new DssException("Failure Customer Details, Code:" + _restClient.LastResponse.StatusCode);
            }

        }
        public async Task<Address[]> GetCustomerAddressDetails(string customerId, HttpRequestMessage request)
        {
            try
            {
                request.Headers.Add("version", _dssSettings.Value.CustomerAddressDetailsApiVersion);
                return await  _restClient.GetAsync<Address[]>(_dssSettings.Value.CustomerAddressDetailsApiUrl.Replace("{customerId}",customerId), request);
            }
            catch (Exception e)
            {
                if (_restClient.LastResponse.StatusCode == HttpStatusCode.NoContent)
                    return null;
                else
                    throw new DssException("Failure Address, Code:" + _restClient.LastResponse.StatusCode);
            }
            
        }

        public async Task<Contact> GetCustomerContactDetails(string customerId, HttpRequestMessage request)
        {
            try
            {
                request.Headers.Add("version", _dssSettings.Value.CustomerContactDetailsApiVersion);
                return await  _restClient.GetAsync<Contact>(_dssSettings.Value.CustomerContactDetailsApiUrl.Replace("{customerId}",customerId), request);
            }
            catch (Exception e)
            {
                if (_restClient.LastResponse.StatusCode == HttpStatusCode.NoContent)
                    return null;
                else
                    throw new DssException("Failure Contact, Code:" + _restClient.LastResponse.StatusCode);
            }
                
        }
    }
}
