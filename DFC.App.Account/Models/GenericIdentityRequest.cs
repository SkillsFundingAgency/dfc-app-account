using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Account.Models
{
    [ExcludeFromCodeCoverage]
    public class GenericIdentityRequest
    {
        public Guid CitizenId { get; set; }
    }
}
