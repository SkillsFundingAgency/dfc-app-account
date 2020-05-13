using System;

namespace DFC.App.Account.Services.SHC.Models
{
    public class ShcException : Exception
    {
        public ShcException(string message)
        :base(message)
        { }
    }
}
