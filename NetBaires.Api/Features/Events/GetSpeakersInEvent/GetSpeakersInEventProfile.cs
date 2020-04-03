using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.GetSpeakersInEvent
{
    public class GetSpeakersInEventProfile : Profile
    {
        public GetSpeakersInEventProfile()
        {
            CreateMap<Attendance, GetSpeakersInEventInEventResponse>();
        }
    }
}