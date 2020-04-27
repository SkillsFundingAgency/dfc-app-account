using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.Account.Services.DSS.Models
{
    public class Contact
    {
        public string ContactId { get; set; }
        public PreferredContactMethod PreferredContactMethod { get; set; }
        public object MobileNumber { get; set; }
        public object HomeNumber { get; set; }
        public object AlternativeNumber { get; set; }
        [EmailAddress(ErrorMessage = "Must be a valid email address")]
        public string EmailAddress { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

}
