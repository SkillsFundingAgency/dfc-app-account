﻿using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Account.Services.DSS.Models
{
    [ExcludeFromCodeCoverage]
    public class DssSettings
    {
        public string CustomerApiUrl { get; set; }
        public string CustomerApiVersion { get; set; }
        
        public string CustomerContactDetailsApiUrl { get; set; }
        public string CustomerContactDetailsApiVersion { get; set; }
        
        public string CustomerAddressDetailsApiUrl { get; set; }
        public string CustomerAddressDetailsApiVersion { get; set; }

        public string ActionPlansApiUrl { get; set; }
        public string ActionPlansApiVersion { get; set; }

        public string AccountsTouchpointId { get; set; }
        public string ApiKey { get; set; }

        public string DigitalIdentitiesPatchByCustomerIdApiUrl { get; set; }
        public string DigitalIdentitiesPatchByCustomerIdApiVersion { get; set; }
    }
}
