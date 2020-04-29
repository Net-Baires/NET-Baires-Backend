using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using NetBaires.Api.Auth;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.ViewModels
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
        public bool Online { get; set; }
        public string OnlineLink { get; set; }
        public int EmailTemplateThanksSponsorsId { get; set; }
        public int EmailTemplateThanksSpeakersId { get; set; }
        public int EmailTemplateThanksAttendedId { get; set; }
        public DateTime Date { get; set; }
        public bool IsUserRegistered { get; set; }
        public bool GeneralAttended { get; set; }
        public int Attended { get; set; }
        public int DidNotAttend { get; set; }
        public int Registered { get; set; }
        public List<SponsorEventViewModel> Sponsors { get; set; }
    
        public class SponsorEventViewModel
        {
            public int SponsorId { get; set; }
            public string Detail { get; set; }
        }
        public class EventDetailViewModelProfile : Profile
        {
            
            public EventDetailViewModelProfile()
            {
                CreateMap<Event, EventDetailViewModel>()
                     .ForMember(dest => dest.IsUserRegistered, o => o.MapFrom<RegisteredCurrentUserInEventResolver>())
                     .ForMember(dest => dest.Attended, o => o.MapFrom(s => s.Attendees.Count(c => c.Attended)))
                     .ForMember(dest => dest.DidNotAttend, o => o.MapFrom(s => s.Attendees.Count(c => c.DidNotAttend)))
                     .ForMember(dest => dest.Registered, o => o.MapFrom(s => s.Attendees.Count));
                CreateMap<SponsorEvent, SponsorEventViewModel>();

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
                return _currentUser.IsLoggued && source.Attendees.Any(a => a.MemberId == _currentUser.User.Id);
            }
        }

    }
}
