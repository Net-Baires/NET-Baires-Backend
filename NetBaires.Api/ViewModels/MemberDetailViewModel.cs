using System;
using System.Linq;
using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.ViewModels
{
    public class MemberDetailViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Twitter { get; set; }
        public string WorkPosition { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public string Github { get; set; }
        public DateTime FirstLogin { get; set; }
        public string Biography { get; set; }
        public string Picture { get; set; }
        public bool Blocked { get; set; }
        public bool Organized { get; set; }
        public bool Colaborator { get; set; }
        public decimal AverageAttendance { get; set; }
        public decimal EventsRegistered { get; set; }
        public decimal EventsAttended { get; set; }
        public decimal EventsNoAttended { get; set; }

        public class MemberDetailViewModelProfile : Profile
        {
            public MemberDetailViewModelProfile()
            {
                CreateMap<Member, MemberDetailViewModel>()
                    .ForMember(dest => dest.AverageAttendance, opt => opt.MapFrom(src => src.Events.Any() ? ((src.Events.Count(e => e.Attended) * 100) / src.Events.Count) : 100))
                    .ForMember(dest => dest.EventsRegistered, opt => opt.MapFrom(src => src.Events.Count))
                    .ForMember(dest => dest.EventsAttended, opt => opt.MapFrom(src => src.Events.Count(e => e.Attended)))
                    .ForMember(dest => dest.EventsNoAttended, opt => opt.MapFrom(src => src.Events.Count(e => e.DidNotAttend)));
            }
        }

    }
}
