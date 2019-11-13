using AutoMapper;
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
        public class GetAttendeesProfile : Profile
        {
            public GetAttendeesProfile()
            {
                CreateMap<Attendance, AttendantViewModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Member.Email))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Member.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Member.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Member.LastName))
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src => src.Member.Picture));

            }

        }
    }
}
