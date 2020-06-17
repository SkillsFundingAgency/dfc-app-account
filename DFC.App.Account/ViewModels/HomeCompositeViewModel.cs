using System;
using DFC.App.Account.Application.SkillsHealthCheck.Models;
using System.Collections.Generic;
using DFC.App.Account.Services.DSS.Models;

namespace DFC.App.Account.ViewModels
{
    public class HomeCompositeViewModel : CompositeViewModel
    {
        public bool HasErrors { get; set; }
        public HomeCompositeViewModel()
            : base(PageId.Home, "Home")
        {
            ShcDocuments = new List<ShcDocument>();
        }

        public List<ShcDocument> ShcDocuments { get; set; }
        public IList<ActionPlan> ActionPlans { get; set; }
        public String ResetPasswordUrl { get; set; }
        public String ActionPlansUrl { get; set; }
    }
}