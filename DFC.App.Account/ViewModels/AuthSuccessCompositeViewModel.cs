using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.Account.ViewModels
{
   
    public class AuthSuccessCompositeViewModel : CompositeViewModel
    {
        public AuthSuccessCompositeViewModel() : base(PageId.AuthSuccess, "Change Password")
        {
        }
    }
}
