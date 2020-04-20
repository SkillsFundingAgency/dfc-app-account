using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.Account.Services.DSS.Models
{
    public class Address
    {
     
        public string AddressId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string PostCode { get; set; }
        public string AlternativePostCode { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public object EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedTouchpointId { get; set; }
        public object SubcontractorId { get; set; }
    }

}
