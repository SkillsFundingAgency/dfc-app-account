using System;
using System.Runtime.Serialization;

namespace DFC.App.Account.Exception
{
    [Serializable]
    public class UserNotValidatedException :  System.Exception, ISerializable
    {
        public UserNotValidatedException(string message)
            : base(message)
        {

        }

    }
}
