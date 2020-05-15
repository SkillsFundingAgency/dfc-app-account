using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.Account.Services.DSS.Models
{
    public class DeleteCustomerRequest
    {
        public Guid CustomerId { get; set; }
        public DateTime DateOfTermination { get; set; }
        public string ReasonForTermination { get; set; }
    }
}
