using System;
using Newtonsoft.Json;

namespace NetBaires.Api.Services.Meetup.Models
{
    public  class MeetupEventDetail
    {
        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public long? Duration { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("date_in_series_pattern")]
        public bool DateInSeriesPattern { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("local_date")]
        public DateTimeOffset LocalDate { get; set; }

        [JsonProperty("local_time")]
        public string LocalTime { get; set; }

        [JsonProperty("updated")]
        public long Updated { get; set; }

        [JsonProperty("utc_offset")]
        public long UtcOffset { get; set; }

        [JsonProperty("waitlist_count")]
        public long WaitlistCount { get; set; }
        [JsonProperty("featured_photo")]
        public FeaturedPhoto FeaturedPhoto { get; set; }

        [JsonProperty("yes_rsvp_count")]
        public long YesRsvpCount { get; set; }

        [JsonProperty("venue", NullValueHandling = NullValueHandling.Ignore)]
        public Venue Venue { get; set; }

        [JsonProperty("group")]
        public Group Group { get; set; }

        [JsonProperty("link")]
        public Uri Link { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("visibility")]
        public string Visibility { get; set; }

        [JsonProperty("member_pay_fee")]
        public bool MemberPayFee { get; set; }

        [JsonProperty("rsvp_limit", NullValueHandling = NullValueHandling.Ignore)]
        public long? RsvpLimit { get; set; }

        [JsonProperty("rsvp_close_offset", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvpCloseOffset { get; set; }

        [JsonProperty("manual_attendance_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? ManualAttendanceCount { get; set; }

        [JsonProperty("how_to_find_us", NullValueHandling = NullValueHandling.Ignore)]
        public string HowToFindUs { get; set; }
    }
}