using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Account.Application.SkillsHealthCheck.Models
{
    [ExcludeFromCodeCoverage]
    public class ShcDocument
    {
        public string DocumentId { get; set; }
        public string LinkUrl { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
