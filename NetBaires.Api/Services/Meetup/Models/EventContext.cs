using Newtonsoft.Json;

namespace NetBaires.Api.Services.Meetup.Models
{
    public class EventContext
    {
        [JsonProperty("host")]
        public bool Host { get; set; }
    }
}