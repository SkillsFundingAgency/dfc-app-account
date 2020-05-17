using System;

namespace DFC.App.Account.ViewModels
{
    public class ShcDeletedCompositeViewModel : CompositeViewModel
    {
        public string DocumentId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public ShcDeletedCompositeViewModel() : base(PageId.ShcDeleted, "Skills health check deleted")
        {
        }
    }
}
