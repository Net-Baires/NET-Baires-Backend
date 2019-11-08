using System;
using System.Text.Json.Serialization;
using NetBaires.Data;

namespace NetBaires.Api.Models
{
    public class GetToSyncResponse
    {
        public int Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventPlatform Platform { get; set; }
        public string Title { get; set; }
        public bool Live { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Date { get; set; }
        public int Attended { get; set; }
        public int DidNotAttend { get; set; }
        public int Registered { get; set; }

        public GetToSyncResponse()
        {

        }
        public GetToSyncResponse(Event @event, int attendedCount, int didNotAttendCount, int registered)
        {
            Attended = attendedCount;
            DidNotAttend = didNotAttendCount;
            Registered = registered;
            Id = @event.Id;
            Platform = @event.Platform;
            Title = @event.Title;
            Date = @event.Date;
            ImageUrl = @event.ImageUrl;
            Live = @event.Live;
        }
    }
}