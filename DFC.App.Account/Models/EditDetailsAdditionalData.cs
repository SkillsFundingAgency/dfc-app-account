using System.Security.Policy;
using DFC.App.Account.Application.Common.Models;

namespace DFC.App.Account.Models
{
    public class EditDetailsAdditionalData
    {
        public string FindAddress { get; set; }
        public string SelectAddress { get; set; }
        public string SaveDetails { get; set; }
        public bool MarketingOptIn { get; set; }
        public bool MarketResearchOptIn { get; set; }
        public PostalAddressModel SelectedAddress { get; set; }
    }
}
