using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace DFC.App.Account.Services.DSS.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class EmailAddressAlreadyExistsException : System.Exception, ISerializable
    {
        public EmailAddressAlreadyExistsException(string message)
            : base(message)
        {

        }

    }
}
