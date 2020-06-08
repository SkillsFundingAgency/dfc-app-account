using System;

namespace DFC.App.Account.Services.DSS.Exceptions
{
    public class DssException : Exception
    {
        public DssException(string message)
            :base(message)
        { }

    }
}
