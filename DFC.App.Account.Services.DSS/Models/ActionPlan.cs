using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.Account.Services.DSS.Models
{
    public class ActionPlan
    {

            public string ActionPlanId { get; set; }
            public string CustomerId { get; set; }
            public string InteractionId { get; set; }
            public string SessionId { get; set; }
            public string SubcontractorId { get; set; }
            public DateTime DateActionPlanCreated { get; set; }
            public string CustomerCharterShownToCustomer { get; set; }
            public DateTime DateAndTimeCharterShown { get; set; }
            public DateTime DateActionPlanSentToCustomer { get; set; }
            public string ActionPlanDeliveryMethod { get; set; }
            public DateTime? DateActionPlanAcknowledged { get; set; }
            public string CurrentSituation { get; set; }
            public DateTime LastModifiedDate { get; set; }
            public string LastModifiedTouchpointId { get; set; }
      
    }

}
