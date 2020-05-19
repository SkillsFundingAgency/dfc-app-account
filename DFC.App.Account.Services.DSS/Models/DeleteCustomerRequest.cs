using System;

namespace DFC.App.Account.Services.DSS.Models
{
    public class DeleteCustomerRequest
    {
        public Guid CustomerId { get; set; }
        public DateTime DateOfTermination { get; set; }
        public int ReasonForTermination { get; set; }
    }
}
