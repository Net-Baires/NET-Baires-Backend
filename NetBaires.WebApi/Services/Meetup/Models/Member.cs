using Newtonsoft.Json;

namespace NetBaires.Api.Services.Meetup.Models
{
    public class Member
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("photo", NullValueHandling = NullValueHandling.Ignore)]
        public Photo Photo { get; set; }

        [JsonProperty("event_context")]
        public EventContext EventContext { get; set; }

        [JsonProperty("bio", NullValueHandling = NullValueHandling.Ignore)]
        public string Bio { get; set; }

        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }
    }
}