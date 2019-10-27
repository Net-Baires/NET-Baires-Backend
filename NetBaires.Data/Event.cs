using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NetBaires.Data
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EventPlatform
    {
        [EnumMember(Value = "Meetup")]
        Meetup,
        [EnumMember(Value = "EventBrite")]
        EventBrite
    }
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public EventPlatform Platform { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public List<Attendance> Attendees { get; set; }
        public string EventId { get; set; }
        public bool Done { get; set; } = false;
        public bool Live { get; set; } = false;
        public DateTime Date { get; set; }
        public List<SponsorEvent> Sponsors { get; set; }

    }
}