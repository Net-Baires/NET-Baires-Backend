using Newtonsoft.Json;

namespace NetBaires.Api.Services.Meetup.Models
{
    public class Rsvp
    {
        [JsonProperty("response")]
        public string Response { get; set; }
    }
}