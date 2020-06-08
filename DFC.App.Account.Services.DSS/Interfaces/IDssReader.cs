using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.Account.Services.DSS.Models;

namespace DFC.App.Account.Services.DSS.Interfaces
{
    public interface IDssReader
    {
        Task<Customer> GetCustomerData(string customerId);
        Task<Customer> GetCustomerDetail(string customerId, HttpRequestMessage request);
        Task<IList<Address>> GetCustomerAddressDetails(string customerId, HttpRequestMessage request);
        Task<Contact> GetCustomerContactDetails(string customerId, HttpRequestMessage request);
        Task<IList<ActionPlan>> GetActionPlans(string customerId);
    }
}
