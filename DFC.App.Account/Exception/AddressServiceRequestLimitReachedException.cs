using System;
using System.Runtime.Serialization;

namespace DFC.App.Account.Exception
{
    [Serializable]
    public class AddressServiceRequestLimitReachedException : System.Exception, ISerializable
    {
        public AddressServiceRequestLimitReachedException(string message)
            : base(message)
        {

        }

    }
}
