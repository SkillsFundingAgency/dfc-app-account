using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Account.Services.SHC.Models
{
    [ExcludeFromCodeCoverage]
    public class ShcSettings
    {
        public string SHCDocType { get; set; }
        public string ServiceName { get; set; }
        public string Url { get; set; }
        public string FindDocumentsAction { get; set; }
        public string LinkUrl { get; set; }
    }
}
