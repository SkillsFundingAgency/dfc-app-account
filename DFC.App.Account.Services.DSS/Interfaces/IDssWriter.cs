using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.Account.Services.DSS.Models;

namespace DFC.App.Account.Services.DSS.Interfaces
{
    public interface IDssWriter
    {
        Task<Customer> CreateCustomerData(Customer customerData);
        Task<Customer> UpdateCustomerData(Customer customerData);
    }
}
