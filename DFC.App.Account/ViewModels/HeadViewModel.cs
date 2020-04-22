using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.Account.ViewModels
{
    public class HeadViewModel
    {
        private const string DefaultPageTitle = "Your Account";

        public string PageTitle { get; set; } = DefaultPageTitle;
    }
}
