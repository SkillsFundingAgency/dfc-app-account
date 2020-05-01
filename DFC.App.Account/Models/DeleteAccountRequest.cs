namespace DFC.App.Account.Models
{
    public class DeleteAccountRequest : GenericIdentityRequest
    {
        public string Password { get; set; }
        public string Reason { get; set; }
    }
}
