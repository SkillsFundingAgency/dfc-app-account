using DFC.App.Account.Services.DSS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Account.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class HomeCompositeViewModel : CompositeViewModel
    {
        public bool HasErrors { get; set; }
        public HomeCompositeViewModel()
            : base(PageId.Home, "Home")
        {
        }

        public IList<ActionPlan> ActionPlans { get; set; }
        public String ResetPasswordUrl { get; set; }
        public String ActionPlansUrl { get; set; }
    }
}