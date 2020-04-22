using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DFC.App.Account.ViewModels
{
    public class ErrorCompositeViewModel : CompositeViewModel
    {
       
        public ErrorCompositeViewModel() : base(PageId.Error, "Service Error")
        {
        }

    }
}
