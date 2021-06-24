using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace DFC.App.Account.Services.Auth.Models
{
    [ExcludeFromCodeCoverage]
    public class OpenIDConnectConfig
    {
        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("authorization_endpoint")]
        public string AuthorizationEndpoint { get; set; }

        [JsonProperty("token_endpoint")]
        public string TokenEndpoint { get; set; }

        [JsonProperty("end_session_endpoint")]
        public string EndSessionEndpoint { get; set; }

        [JsonProperty("jwks_uri")]
        public string JwksUri { get; set; }
    }
}
