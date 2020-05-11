using DFC.App.Account.Application.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.Account.Services.Interfaces
{
    public interface IAddressSearchService
    {
        Task<IEnumerable<PostalAddressModel>> GetAddresses(string postalCode);
        Task<PostalAddressModel> GetAddress(string addressIdentifier);
    }
}
