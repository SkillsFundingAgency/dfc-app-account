using System.Collections.Generic;
using DFC.App.Account.Application.SkillsHealthCheck.Models;

namespace DFC.App.Account.ViewModels
{
    public class HomeCompositeViewModel : CompositeViewModel
    {
        public bool HasErrors { get; set; }
        public HomeCompositeViewModel()
            : base(PageId.Home, "Home")
        {
            ShcDocuments = new List<ShcDocuments>();
        }

        public List<ShcDocuments> ShcDocuments { get; set; }
    }
}