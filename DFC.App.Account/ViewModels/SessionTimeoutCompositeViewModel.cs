using System;

namespace DFC.App.Account.ViewModels
{
    public class SessionTimeoutCompositeViewModel : CompositeViewModel
    {
        public SessionTimeoutCompositeViewModel() : base(PageId.SessionTimeout, "Session Timeout")
        {
        }

        public String SignInUrl { get; set; }
    }
}
