using System;

namespace NetBaires.Api.Models
{
    public class EventAssistanceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Date { get; set; }
        public string Token { get; set; }
    }
}