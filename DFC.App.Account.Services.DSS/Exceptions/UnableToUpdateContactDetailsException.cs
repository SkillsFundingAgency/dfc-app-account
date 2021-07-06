using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace DFC.App.Account.Services.DSS.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class UnableToUpdateContactDetailsException : System.Exception, ISerializable
    {
        public UnableToUpdateContactDetailsException(string message)
            : base(message)
        {

        }

    }
}
