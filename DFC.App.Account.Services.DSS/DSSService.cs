using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.Account.Services.DSS.Models;
using DFC.Personalisation.Common.Net.RestClient;

namespace DFC.App.Account.Services.DSS
{
    public class DSSService
    {
        private IRestClient _restClient;

        public DSSService(IRestClient restClient)
        {
            _restClient = restClient;
        }


        public async Task<Customer> GetCustomer(string customerId, string touchPointId)
        {
            
            var request = await BuidRequestMessage(customerId, touchPointId);
            
            var customer = new Customer();
            
            customer = await GetCustomerDetail(customerId, request);
            customer.Addresses = await GetCustomerAddressDetails(customerId, request);
            customer.Contacts = await GetCustomerContactDetails(customerId, request);

            return customer;

        }

        private async Task<HttpRequestMessage> BuidRequestMessage(string customerId, string touchPointId)
        {
            if (customerId == null) throw new ArgumentNullException(nameof(customerId));
            if (touchPointId == null) throw new ArgumentNullException(nameof(touchPointId));

            var request = new HttpRequestMessage();
            var ocpApimSubscriptionKey = "ef6a32a229834291b69105a690c17413";
            request.Headers.Add("Ocp-Apim-Subscription-Key", ocpApimSubscriptionKey);
            request.Headers.Add("TouchpointId", touchPointId);
            
            return request;
        }

        private async Task<Customer> GetCustomerDetail(string customerId, HttpRequestMessage request)
        {
            try
            {
                var version = "v2";
                request.Headers.Add("version", version);
                return await  _restClient.GetAsync<Customer>($"https://at.api.nationalcareersservice.org.uk/customers/api/Customers/{customerId}", request);
            }
            catch (Exception e)
            {
                throw new DSSException("Failure Customer Details, Code:" + _restClient.LastResponse.StatusCode);
            }

        }
        private async Task<Address[]> GetCustomerAddressDetails(string customerId, HttpRequestMessage request)
        {
            try
            {
                var version = "v2";
                request.Headers.Add("version", version);
                return await  _restClient.GetAsync<Address[]>($"https://at.api.nationalcareersservice.org.uk/addresses/api/Customers/{customerId}/Addresses", request);
            }
            catch (Exception e)
            {
                throw new DSSException("Failure Address, Code:" + _restClient.LastResponse.StatusCode);
            }
            
        }

        private async Task<Contact> GetCustomerContactDetails(string customerId, HttpRequestMessage request)
        {
            try
            {
                var version = "v2";
                request.Headers.Add("version", version);
                return await  _restClient.GetAsync<Contact>($"https://at.api.nationalcareersservice.org.uk/contactdetails/api/customers/{customerId}/ContactDetails/", request);
            }
            catch (Exception e)
            {
                throw new DSSException("Failure Contact, Code:" + _restClient.LastResponse.StatusCode);
            }
                
        }
        
    }


}
