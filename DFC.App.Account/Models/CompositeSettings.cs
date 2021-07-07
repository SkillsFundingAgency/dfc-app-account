using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Account.Models
{
    [ExcludeFromCodeCoverage]
    public class CompositeSettings
    {
        // Path that is registered for this application in the Cosmos 'paths' collection
        public string Path { get; set; }
        public string CDN { get; set; }
    }
}
