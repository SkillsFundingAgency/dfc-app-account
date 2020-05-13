using System;
using System.Runtime.Serialization;

namespace DFC.App.Account.Services.DSS.Exceptions
{
    [Serializable]
    public class UnableToUpdateCustomerDetailsException : System.Exception, ISerializable
    {
        public UnableToUpdateCustomerDetailsException(string message)
            : base(message)
        {

        }

    }
}
