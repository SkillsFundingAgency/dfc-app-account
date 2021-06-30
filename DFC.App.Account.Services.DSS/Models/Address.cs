using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Account.Services.DSS.Models
{
    [ExcludeFromCodeCoverage]
    public class Address
    {
     
        public string AddressId { get; set; }
        public string Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string? Address4 { get; set; }
        public string? Address5 { get; set; }
        public string PostCode { get; set; }
        public string? AlternativePostCode { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public DateTimeOffset? EffectiveFrom { get; set; }
        public DateTimeOffset? EffectiveTo { get; set; }
        public DateTimeOffset LastModifiedDate { get; set; }
        public string LastModifiedTouchpointId { get; set; }
        public string SubcontractorId { get; set; }
    }

}
