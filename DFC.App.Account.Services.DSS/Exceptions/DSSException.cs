using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Account.Services.DSS.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class DssException : Exception
    {
        public DssException(string message)
            :base(message)
        { }

    }
}
