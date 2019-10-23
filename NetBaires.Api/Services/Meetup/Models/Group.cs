using Newtonsoft.Json;

namespace NetBaires.Api.Services.Meetup.Models
{
    public partial class Group
    {
        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("join_mode")]
        public string JoinMode { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("urlname")]
        public string Urlname { get; set; }

        [JsonProperty("who")]
        public string Who { get; set; }

        [JsonProperty("localized_location")]
        public string LocalizedLocation { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }
    }
}