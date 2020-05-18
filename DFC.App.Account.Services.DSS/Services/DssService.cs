using Dfc.ProviderPortal.Packages;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.Account.Services.DSS.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using DFC.App.Account.Application.Common;
using DFC.App.Account.Application.Common.Interfaces;
using DFC.App.Account.Services.DSS.Exceptions;

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
            if (customerData == null)
                return null;


            Customer result;
            using (var request = CreateRequestMessage())
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(customerData),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json);
                request.Headers.Add("version", _dssSettings.Value.CustomerApiVersion);

                result = await _restClient.PostAsync<Customer>(_dssSettings.Value.CustomerApiUrl, request);
            }

            return _restClient.LastResponse.IsSuccess ? result : null;
        }

        public async Task<Customer> UpdateCustomerData(Customer customerData)
        {
            if (customerData == null)
                return null;

            Customer result;
            using (var request = CreateRequestMessage())
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(customerData),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json);
                request.Headers.Add("version", _dssSettings.Value.CustomerApiVersion);

                result = await _restClient.PatchAsync<Customer>(apiPath:$"{_dssSettings.Value.CustomerApiUrl}{customerData.CustomerId}", requestMessage: request);
            }

            if (!_restClient.LastResponse.IsSuccess)
            {
                throw new UnableToUpdateCustomerDetailsException($"Unable To Updated customer details for customer {customerData.CustomerId}, Response {_restClient.LastResponse.Content}");
            }

            return result;
        }

        public async Task<Contact> UpsertCustomerContactData(Customer customerData)
        {
            if (customerData == null)
                return null;
            Contact result;
            using (var request = CreateRequestMessage())
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(customerData.Contact),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json);
                request.Headers.Add("version", _dssSettings.Value.CustomerContactDetailsApiVersion);


                if (string.IsNullOrEmpty(customerData.Contact.ContactId))
                {
                    result = await _restClient.PostAsync<Contact>(apiPath: _dssSettings.Value.CustomerContactDetailsApiUrl.Replace("{customerId}", customerData.CustomerId.ToString()), 
                        requestMessage: request);
                }
                else
                {
                    result = (await _restClient.PatchAsync<IList<Contact>>(apiPath: _dssSettings.Value.CustomerContactDetailsApiUrl.Replace("{customerId}", customerData.CustomerId.ToString()) + 
                                                                            customerData.Contact.ContactId, requestMessage: request)).FirstOrDefault();
                }
            }

            if (_restClient.LastResponse.Content.Contains(
                $"Contact with Email Address {customerData.Contact.EmailAddress} already exists"))
            {
                throw new EmailAddressAlreadyExistsException(_restClient.LastResponse.Content);
            }

            if (!_restClient.LastResponse.IsSuccess)
            {
              throw new UnableToUpdateContactDetailsException($"Unable To Updated contact details for customer {customerData.CustomerId}, Response {_restClient.LastResponse.Content}");
            }

            return result;
        }

        public async Task<Address> UpsertCustomerAddressData(Address address, Guid customerId)
        {
            Address result;
            using (var request = CreateRequestMessage())
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(address));
                request.Headers.Add("version", _dssSettings.Value.CustomerAddressDetailsApiVersion);


                if (string.IsNullOrEmpty(address.AddressId))
                {
                    result = await _restClient.PostAsync<Address>(apiPath: _dssSettings.Value.CustomerAddressDetailsApiUrl.Replace("{customerId}", customerId.ToString()),
                        requestMessage: request);
                }
                else
                {
                    result = await _restClient.PatchAsync<Address>(apiPath: _dssSettings.Value.CustomerAddressDetailsApiUrl.Replace("{customerId}", customerId.ToString()) +
                                                                                                 address.AddressId, requestMessage: request);
                }
            }

            if (!_restClient.LastResponse.IsSuccess)
            {
                throw new UnableToUpdateAddressDetailsException($"Unable To Updated address details for customer {customerId}, Response {_restClient.LastResponse.Content}");
            }

            return result;
        }

        public async Task<Customer> GetCustomerData(string customerId)
        {

            var request = CreateRequestMessage();
            request.Headers.Add("version", _dssSettings.Value.CustomerApiVersion);

            var customer = await GetCustomerDetail(customerId, request);
            customer.Addresses = await GetCustomerAddressDetails(customerId, request);
            customer.Contact = await GetCustomerContactDetails(customerId, request);

            return customer;

        }

        private HttpRequestMessage CreateRequestMessage()
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
                return await _restClient.GetAsync<Customer>($"{_dssSettings.Value.CustomerApiUrl}{customerId}",
                    request);
            }
            catch (Exception e)
            {
                throw new DssException($"Failure Customer Details, Code:{_restClient.LastResponse.StatusCode} {Environment.NewLine}  {e.InnerException}");
            }

        }

        public async Task<IList<Address>> GetCustomerAddressDetails(string customerId, HttpRequestMessage request)
        {
            try
            {
                request.Headers.Add("version", _dssSettings.Value.CustomerAddressDetailsApiVersion);
                return await _restClient.GetAsync<IList<Address>>(_dssSettings.Value.CustomerAddressDetailsApiUrl.Replace("{customerId}", customerId), request);
            }
            catch (Exception e)
            {
                if (_restClient.LastResponse.StatusCode == HttpStatusCode.NoContent)
                    return null;
                else
                    throw new DssException($"Failure Address, Code:{_restClient.LastResponse.StatusCode} {Environment.NewLine}  {e.InnerException}");
            }

        }

        public async Task<Contact> GetCustomerContactDetails(string customerId, HttpRequestMessage request)
        {
            try
            {
                request.Headers.Add("version", _dssSettings.Value.CustomerContactDetailsApiVersion);
                return await _restClient.GetAsync<Contact>(
                    _dssSettings.Value.CustomerContactDetailsApiUrl.Replace("{customerId}", customerId), request)??new Contact();
            }
            catch (Exception e)
            {
                if (_restClient.LastResponse.StatusCode == HttpStatusCode.NoContent)
                    return new Contact();
                else
                    throw new DssException($"Failure Contact, Code:{_restClient.LastResponse.StatusCode} {Environment.NewLine}  {e.InnerException}");
            }

        }

        public async Task<IResult> DeleteCustomer(DeleteCustomerRequest deleteRequest)
        {
            if (deleteRequest == null)
                throw new UnableToUpdateCustomerDetailsException(
                    $"Unable To Updated customer details for customer. No details provided.");
            try
            {
                using (var request = CreateRequestMessage())
                {
                    request.Content = new StringContent(
                        JsonConvert.SerializeObject(deleteRequest),
                        Encoding.UTF8,
                        MediaTypeNames.Application.Json);
                    request.Headers.Add("version", _dssSettings.Value.CustomerApiVersion);
                    var result = await _restClient.PatchAsync<DeleteCustomerRequest>(
                        apiPath: $"{_dssSettings.Value.CustomerApiUrl}{deleteRequest.CustomerId}",
                        requestMessage: request);
                }

                if (!_restClient.LastResponse.IsSuccess)
                {
                    throw new UnableToUpdateCustomerDetailsException(
                        $"Unable To Updated customer details for customer {deleteRequest.CustomerId}, Response {_restClient.LastResponse.Content}");
                }

                return Result.Ok();
            }
            catch (Exception e)
            {
                throw new UnableToUpdateCustomerDetailsException(
                    $"Unable To Updated customer details for customer {deleteRequest.CustomerId}, Response {_restClient.LastResponse.Content}");
            }
        }
    }
}