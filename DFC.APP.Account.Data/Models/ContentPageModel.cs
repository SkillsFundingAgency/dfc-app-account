using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DFC.APP.Account.Data.Extensions;

namespace DFC.APP.Account.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class ContentPageModel : Compui.Cosmos.Models.ContentPageModel
    {
        [Required]
        public override string? PartitionKey => PageLocation;

        public override string? PageLocation { get; set; } = "/missing-location";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public new Guid? Version { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public new string? Content { get; set; }

        public bool ShowHeroBanner { get; set; }

        public string? HeroBanner { get; set; }

        public List<ContentItemModel>? ContentItems { get; set; } = new List<ContentItemModel>();

        [JsonIgnore]
        public List<Guid> AllContentItemIds
        {
            get
            {
                var result = new List<Guid>();

                if (ContentItems != null)
                {
                    result.AddRange(ContentItems.Flatten(s => s.ContentItems).Where(w => w.ItemId != null).Select(s => s.ItemId!.Value));
                }

                return result;
            }
        }
    }
}
