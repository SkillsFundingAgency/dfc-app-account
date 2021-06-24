using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DFC.App.Account.Application.Common.Enums;

namespace DFC.App.Account.Services.DSS.Models
{
    [ExcludeFromCodeCoverage]
    public class Contact
    {
        public string ContactId { get; set; }
        public CommonEnums.Channel PreferredContactMethod { get; set; }
        public string MobileNumber { get; set; }
        public string HomeNumber { get; set; }
        public string AlternativeNumber { get; set; }
        [EmailAddress(ErrorMessage = "Must be a valid email address")]
        public string EmailAddress { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

}
