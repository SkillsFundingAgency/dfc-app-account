using System;

namespace DFC.App.Account.ViewModels
{
    public class DeleteAccountCompositeViewModel : CompositeViewModel
    {
        public Guid CustomerId { get; set; }
        public DeleteAccountCompositeViewModel() : base(PageId.DeleteAccount, "Account closed")
        {
           
        }



    }

}
