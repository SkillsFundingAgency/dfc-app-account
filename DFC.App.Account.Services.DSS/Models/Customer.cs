using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.Account.Services.DSS.Models
{
    public class Customer
    {

        public string CustomerId { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public int Title { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public DateTime DateofBirth { get; set; }
        public int Gender { get; set; }
        public string UniqueLearnerNumber { get; set; }
        public bool OptInUserResearch { get; set; }
        public bool OptInMarketResearch { get; set; }
        public object DateOfTermination { get; set; }
        public object ReasonForTermination { get; set; }
        public int IntroducedBy { get; set; }
        public string IntroducedByAdditionalInfo { get; set; }
        public string SubcontractorId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedTouchpointId { get; set; }
        public Address[] Addresses { get; set; }
        public Contact[] Contacts { get; set; }
    }

}
