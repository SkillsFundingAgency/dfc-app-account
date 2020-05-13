using System.Collections.Generic;
using Newtonsoft.Json;

namespace DFC.App.Account.Models
{
    public class GetAddressIOAddressModel
    {
        [JsonProperty("line_1")]
        public string Line1 { get; set; }

        [JsonProperty("line_2")]
        public string Line2 { get; set; }

        [JsonProperty("line_3")]
        public string Line3 { get; set; }

        [JsonProperty("line_4")]
        public string Line4 { get; set; }

        [JsonProperty("town_or_city")]
        public string City { get; set; }

        public string County { get; set; }

        [JsonProperty("formatted_address")]
        public IList<string> FormattedAddress { get; set; }
    }
}
