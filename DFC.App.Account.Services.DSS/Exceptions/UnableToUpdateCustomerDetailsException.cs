using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace DFC.App.Account.Services.DSS.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class UnableToUpdateCustomerDetailsException : System.Exception, ISerializable
    {
        public UnableToUpdateCustomerDetailsException(string message)
            : base(message)
        {

        }

    }
}
