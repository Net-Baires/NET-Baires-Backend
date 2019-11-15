using AutoMapper;
using NetBaires.Api.Features.Members.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.ViewModels
{
    public class AttendantViewModel
    {
        public int Id { get; set; }
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
        public MemberDetailViewModel MemberDetail { get; set; }
        public class GetAttendeesProfile : Profile
        {
            public GetAttendeesProfile()
            {
                CreateMap<Attendance, AttendantViewModel>()
                    .ForMember(dest => dest.MemberDetail, opt => opt.MapFrom(src => src.Member));

            }

        }
    }
}
