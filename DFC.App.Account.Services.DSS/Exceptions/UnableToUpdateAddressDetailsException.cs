using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace DFC.App.Account.Services.DSS.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class UnableToUpdateAddressDetailsException : System.Exception, ISerializable
    {
        public UnableToUpdateAddressDetailsException(string message)
            : base(message)
        {

        }

    }
}
