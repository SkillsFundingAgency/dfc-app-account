using System;
using System.Runtime.Serialization;

namespace DFC.App.Account.Services.DSS.Exceptions
{
    [Serializable]
    public class EmailAddressAlreadyExistsException : System.Exception, ISerializable
    {
        public EmailAddressAlreadyExistsException(string message)
            : base(message)
        {

        }

    }
}
