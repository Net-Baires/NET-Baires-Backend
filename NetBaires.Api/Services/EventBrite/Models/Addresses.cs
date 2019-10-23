using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{
    public class AttendeesResponse
    {
        public Pagination Pagination { get; set; }
        public List<Attendee> Attendees { get; set; }
    }
    public partial class Profile
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
    public class Attendee
    {
        public Profile profile { get; set; }
        [JsonProperty("checked_in")]
        public bool CheckIn { get; set; }
    }
    public class Event
    {
        public Description Name { get; set; }
        public Description Description { get; set; }
        public End Start { get; set; }

        public End End { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
        public Logo Logo { get; set; }
    }
    public class Logo
    {
        public Original Original { get; set; }
    }



    public class Original
    {
        public Uri Url { get; set; }
    }
    public class Description
    {
        public string Text { get; set; }

        public string Html { get; set; }
    }
    public class End
    {
        public string Timezone { get; set; }

        public DateTimeOffset Local { get; set; }

        public DateTimeOffset Utc { get; set; }
    }
    public class EventsResponse
    {
        public Pagination Pagination { get; set; }

        public Event[] Events { get; set; }

    }
    public class Pagination
    {
        public long ObjectCount { get; set; }
        public long PageNumber { get; set; }
        public long PageSize { get; set; }
        public long PageCount { get; set; }
        public bool HasMoreItems { get; set; }
    }
}