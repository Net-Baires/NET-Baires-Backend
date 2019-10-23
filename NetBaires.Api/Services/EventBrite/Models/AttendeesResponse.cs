using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{

    public class AttendeesResponse
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("attendees")]
        public Attendee[] Attendees { get; set; }
    }


}