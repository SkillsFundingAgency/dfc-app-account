using System;

namespace DFC.App.Account.Services.DSS.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public DateTimeOffset DateOfRegistration { get; set; }
        public long Title { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public DateTimeOffset DateofBirth { get; set; }
        public long Gender { get; set; }
        public string UniqueLearnerNumber { get; set; }
        public bool OptInUserResearch { get; set; }
        public bool OptInMarketResearch { get; set; }
        public DateTimeOffset DateOfTermination { get; set; }
        public long ReasonForTermination { get; set; }
        public long IntroducedBy { get; set; }
        public string IntroducedByAdditionalInfo { get; set; }
        public string SubcontractorId { get; set; }
        public DateTimeOffset LastModifiedDate { get; set; }
        public string LastModifiedTouchpointId { get; set; }
        public Address[] Addresses { get; set; }
        public Contact Contacts { get; set; }
    }

}
