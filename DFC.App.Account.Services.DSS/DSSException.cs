using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.Account.Services.DSS
{
    public class DSSException : Exception
    {
        public DSSException(string message)
            :base(message)
        { }

    }
}
