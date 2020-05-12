using System;

namespace DFC.App.Account.ViewModels
{
    public class ConfirmDeleteCompositeViewModel : CompositeViewModel
    {
        public ConfirmDeleteCompositeViewModel() : base(PageId.ConfirmDelete, "Do you want to delete this Skills health check?")
        {

        }

        public string DocumentId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
