using System;

namespace DFC.App.Account.Application.SkillsHealthCheck.Models
{
    public class ShcDocument
    {
        public string DocumentId { get; set; }
        public string LinkUrl { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
