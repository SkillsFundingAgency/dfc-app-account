using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.Account.ViewModels
{
    public class TimeoutCompositeViewModel : CompositeViewModel
    {
        public TimeoutCompositeViewModel(PageId pageId, string pageHeading) : base(PageId.ChangePassword, "Timeout")
        {
        }
    }
}
