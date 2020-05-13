using DFC.App.Account.Application.Common.Models;
using System.Collections.Generic;

namespace DFC.App.Account.ViewModels
{
    public class PostalAddressViewModel
    {
        public IList<PostalAddressModel> Items { get; set; }
        public PostalAddressModel SelectedItem { get; set; }
    }
}
