using System;
using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{
    public class Original
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }
    }
}