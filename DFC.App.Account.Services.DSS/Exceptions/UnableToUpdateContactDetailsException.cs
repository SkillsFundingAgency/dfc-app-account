using System;
using System.Runtime.Serialization;

namespace DFC.App.Account.Services.DSS.Exceptions
{
    [Serializable]
    public class UnableToUpdateContactDetailsException : System.Exception, ISerializable
    {
        public UnableToUpdateContactDetailsException(string message)
            : base(message)
        {

        }

    }
}
