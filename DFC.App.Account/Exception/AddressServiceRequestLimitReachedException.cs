using System;
using System.Runtime.Serialization;

namespace DFC.App.Account.Exception
{
    [Serializable]
    public class AddressServiceRequestLimitReachedException : System.Exception
    {
        protected AddressServiceRequestLimitReachedException(SerializationInfo info, StreamingContext context)
        {

        }

        public AddressServiceRequestLimitReachedException(string message)
            : base(message)
        {

        }

    }
}
