using System;
using System.Runtime.Serialization;

namespace DFC.App.Account.Exception
{
    [Serializable]
    public class NoUserIdInClaimException :  System.Exception, ISerializable
    {
        public NoUserIdInClaimException(string message)
            : base(message)
        {

        }

    }
}
