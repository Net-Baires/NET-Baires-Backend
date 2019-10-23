using Newtonsoft.Json;

namespace NetBaires.Api.Services.Meetup.Models
{
    public class AttendanceResponse
    {
        [JsonProperty("rsvp")]
        public Rsvp Rsvp { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty("member")]
        public Member Member { get; set; }
    }
}