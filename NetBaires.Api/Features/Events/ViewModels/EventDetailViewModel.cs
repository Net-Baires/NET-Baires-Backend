using AutoMapper;
using NetBaires.Data;
using System;

namespace NetBaires.Api.Features.Events.ViewModels
{
    public class EventDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public EventPlatform Platform { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public string EventId { get; set; }
        public bool Done { get; set; }
        public bool Live { get; set; }
        public DateTime Date { get; set; }
        public class EventDetailViewModelProfile : Profile
        {
            public EventDetailViewModelProfile()
            {
                CreateMap<Event, EventDetailViewModel>();
            }
        }
    }
}
