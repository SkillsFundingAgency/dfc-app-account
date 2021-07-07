using System;
using System.Threading.Tasks;
using DFC.App.Account.Application.Common.Interfaces;
using DFC.App.Account.Services.DSS.Models;

namespace DFC.App.Account.Services.DSS.Interfaces
{
    public interface IDssWriter
    {
        Task<Customer> CreateCustomerData(Customer customerData);
        Task<Customer> UpdateCustomerData(Customer customerData);
        Task UpsertCustomerContactData(Customer customerData);
        Task<IResult> DeleteCustomer(Guid customerId);
        Task<IResult> UpdateLastLogin(Guid customerId, string token);
    }
}
