using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{
    public class Pagination
    {
        [JsonProperty("object_count")]
        public long ObjectCount { get; set; }

        [JsonProperty("page_number")]
        public long PageNumber { get; set; }

        [JsonProperty("page_size")]
        public long PageSize { get; set; }

        [JsonProperty("page_count")]
        public long PageCount { get; set; }

        [JsonProperty("continuation")]
        public string Continuation { get; set; }

        [JsonProperty("has_more_items")]
        public bool HasMoreItems { get; set; }
    }
}