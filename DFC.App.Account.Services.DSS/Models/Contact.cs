using System;

namespace DFC.App.Account.Services.DSS.Models
{
    public class Contact
    {
        public string ContactId { get; set; }
        public int PreferredContactMethod { get; set; }
        public object MobileNumber { get; set; }
        public object HomeNumber { get; set; }
        public object AlternativeNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

}
