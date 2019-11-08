using System;
using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{
    public class Logo
    {
        [JsonProperty("crop_mask")]
        public CropMask CropMask { get; set; }

        [JsonProperty("original")]
        public Original Original { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("aspect_ratio")]
        public long AspectRatio { get; set; }

        [JsonProperty("edge_color")]
        public string EdgeColor { get; set; }

        [JsonProperty("edge_color_set")]
        public bool EdgeColorSet { get; set; }
    }
}