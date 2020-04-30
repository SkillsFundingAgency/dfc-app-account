using System.Collections.Generic;
using DFC.App.Account.Application.Common.Models;

namespace DFC.App.Account.ViewModels
{
    public class EditDetailsCompositeViewModel : CompositeViewModel
    {
        public EditDetailsCompositeViewModel() : base(PageId.EditDetails, "Edit Details")
        {
            
        }
        public CitizenIdentity Identity { get; set; }

        public IList<PostalAddressModel> Items { get; set; }
    }
}
