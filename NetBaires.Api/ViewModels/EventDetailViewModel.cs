using AutoMapper;
using NetBaires.Api.Auth;
using NetBaires.Data;
using System;
using System.Linq;

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
        public bool IsUserRegistered { get; set; }
        public int Attended { get; set; }
        public int DidNotAttend { get; set; }
        public int Registered { get; set; }

        public class EventDetailViewModelProfile : Profile
        {
            public EventDetailViewModelProfile()
            {
                CreateMap<Event, EventDetailViewModel>()
                     .ForMember(dest => dest.IsUserRegistered, o => o.MapFrom<RegisteredCurrentUserInEventResolver>())
                     .ForMember(dest => dest.Attended, o => o.MapFrom(s => s.Attendees.Count(c => c.Attended)))
                     .ForMember(dest => dest.DidNotAttend, o => o.MapFrom(s => s.Attendees.Count(c => c.DidNotAttend)))
                     .ForMember(dest => dest.Registered, o => o.MapFrom(s => s.Attendees.Count()));
            }
        }
        public class RegisteredCurrentUserInEventResolver : IValueResolver<Event, EventDetailViewModel, bool>
        {
            ICurrentUser _currentUser;
            public RegisteredCurrentUserInEventResolver(ICurrentUser currentUser)
            {
                _currentUser = currentUser;
            }
            public bool Resolve(Event source, EventDetailViewModel destination, bool destMember, ResolutionContext context)
            {
                return source.Attendees.Any(a => a.MemberId == _currentUser.User.Id);
            }
        }
     
    }
}
