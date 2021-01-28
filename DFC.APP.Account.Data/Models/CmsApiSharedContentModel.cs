using System;
using DFC.APP.Account.Data.Contracts;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Models;
using Newtonsoft.Json;

namespace DFC.APP.Account.Data.Models
{
    public class CmsApiSharedContentModel : BaseContentItemModel, ICmsApiMarkupContentItem, IDocumentModel
    {
        public int? Justify { get; set; }

        public string? Alignment { get; set; }

        public int? Ordinal { get; set; }

        public int? Size { get; set; }

        public string? Href { get; set; }

        public string? Content { get; set; }

        [JsonProperty("htmlbody_Html")]
        public string? HtmlBody { get; set; }

        public void AddParentId(string parentId)
        {
        }

        public void AddTraceId(string traceId)
        {
        }

        public string? TraceId { get; }
        public string? ParentId { get; }

        public Guid Id { get; set; }

        public string? Etag { get; set; }
        public string? PartitionKey
        {
            get => "account";
            set => value = "account";
        }
    }
}
