using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace DFC.App.Account.Services.DSS.Models
{
    class UpdateLastLoginRequest
    {
        public DateTime LastLoggedInDateTime { get; set; }
        [JsonProperty("id_token")]
        public string Token { get; set; } 
    }
}
