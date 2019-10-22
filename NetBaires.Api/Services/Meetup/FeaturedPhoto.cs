using System;
using Newtonsoft.Json;

namespace NetBaires.Api.Services.Meetup
{
    public  class FeaturedPhoto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("highres_link")]
        public Uri HighresLink { get; set; }

        [JsonProperty("photo_link")]
        public Uri PhotoLink { get; set; }

        [JsonProperty("thumb_link")]
        public Uri ThumbLink { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("base_url")]
        public Uri BaseUrl { get; set; }
    }
}