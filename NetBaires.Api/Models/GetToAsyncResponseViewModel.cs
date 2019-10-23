using System;
using System.Text.Json.Serialization;
using NetBaires.Data;

namespace NetBaires.Api.Models
{
    public class GetToAsyncResponseViewModel
    {
        public int Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventPlatform Platform { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Date { get; set; }
        public int AttendedCount { get; set; }
        public int DidNotAttendCount { get; set; }

        public GetToAsyncResponseViewModel(Event @event, int attendedCount, int didNotAttendCount)
        {
            AttendedCount = attendedCount;
            DidNotAttendCount = didNotAttendCount;
            Id = @event.Id;
            Platform = @event.Platform;
            Title = @event.Title;
            Date = @event.Date;
            ImageUrl = @event.ImageUrl;
        }
    }
}