using System;
using System.Runtime.Serialization;

namespace DFC.App.Account.Services.DSS.Exceptions
{
    [Serializable]
    public class UnableToUpdateAddressDetailsException : System.Exception, ISerializable
    {
        public UnableToUpdateAddressDetailsException(string message)
            : base(message)
        {

        }

    }
}
