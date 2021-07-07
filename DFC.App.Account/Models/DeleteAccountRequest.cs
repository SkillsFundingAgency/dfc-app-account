using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Account.Models
{
    [ExcludeFromCodeCoverage]
    public class DeleteAccountRequest : GenericIdentityRequest
    {
        public string Password { get; set; }
        public string Reason { get; set; }
    }
}
