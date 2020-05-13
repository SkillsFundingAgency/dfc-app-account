using DFC.App.Account.Application.SkillsHealthCheck.Models;
using System.Collections.Generic;

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
    }
}