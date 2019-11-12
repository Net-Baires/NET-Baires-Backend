using System;

namespace NetBaires.Api.Handlers.Events
{
    public class GetEventsResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public string EventId { get; set; }
        public bool Done { get; set; } = false;
        public bool Live { get; set; } = false;
        public DateTime Date { get; set; }
    }

}