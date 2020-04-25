using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.Account.Services.DSS.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public long Title { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        
        public DateTime? DateofBirth { get; set; }
        public long Gender { get; set; }
        public string? UniqueLearnerNumber { get; set; }
        public bool OptInUserResearch { get; set; }
        public bool OptInMarketResearch { get; set; }
        public DateTime? DateOfTermination { get; set; }
        public long? ReasonForTermination { get; set; }
        public long? IntroducedBy { get; set; }
        public string? IntroducedByAdditionalInfo { get; set; }
        public string SubcontractorId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedTouchpointId { get; set; }
        public Address[]? Addresses { get; set; }
        public Contact Contact { get; set; }

    }

}
