using System;
using System.ComponentModel.DataAnnotations;
using DFC.App.Account.Application.Common.Enums;

namespace DFC.App.Account.Services.DSS.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public CommonEnums.Title Title { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateofBirth { get; set; }
        public CommonEnums.Gender Gender { get; set; }
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
