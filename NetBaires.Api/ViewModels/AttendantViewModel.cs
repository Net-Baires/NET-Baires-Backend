using System.Linq;
using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.ViewModels
{
    public class AttendantViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public bool DidNotAttend { get; set; }
        public bool Attended { get; set; }
        public bool NotifiedAbsence { get; set; }
        public bool DoNotKnow { get; set; }
        public bool Organizer { get; set; }
        public bool Speaker { get; set; }
        public long  AverageAttendance { get; set; }
        public MemberDetailViewModel MemberDetail { get; set; }
        public EventDetailViewModel Event { get; set; }
        public class GetAttendeesProfile : Profile
        {
            public GetAttendeesProfile()
            {
                CreateMap<Attendance, AttendantViewModel>()
                    .ForMember(dest => dest.AverageAttendance, opt => opt.MapFrom(src => (src.Member.Events.Count(e => e.Attended) * 100) / src.Member.Events.Count))
                    .ForMember(dest => dest.MemberDetail, opt => opt.MapFrom(src => src.Member));

            }

        }
    }
}
