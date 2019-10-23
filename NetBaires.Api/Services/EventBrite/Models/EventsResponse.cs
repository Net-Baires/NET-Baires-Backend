using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{
    public class EventsResponse
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("events")]
        public Event[] Events { get; set; }
    }
}