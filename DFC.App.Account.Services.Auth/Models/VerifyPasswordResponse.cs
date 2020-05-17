using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DFC.App.Account.Services.Auth.Models
{
    public class VerifyPasswordResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
    }
}
