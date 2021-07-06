using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Account.Models
{
    [ExcludeFromCodeCoverage]
    public class EndPoint
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Methods { get; set; }
    }
}
