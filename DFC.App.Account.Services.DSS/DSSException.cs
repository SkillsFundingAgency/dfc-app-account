using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.Account.Services.DSS
{
    public class DssException : Exception
    {
        public DssException(string message)
            :base(message)
        { }

    }
}
