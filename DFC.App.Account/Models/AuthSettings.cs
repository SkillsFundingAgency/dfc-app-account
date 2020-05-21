namespace DFC.App.Account.Models
{
    public class AuthSettings
    {
        public string ClientSecret { get; set; }
        public string Issuer { get; set; }
        public string ClientId { get; set; }
        public string SignInUrl { get; set; }
        public string SignOutUrl { get; set; }
        public string RegisterUrl { get; set; }
    }
}
