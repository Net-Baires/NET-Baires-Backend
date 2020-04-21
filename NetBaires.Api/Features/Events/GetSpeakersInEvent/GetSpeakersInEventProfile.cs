using AutoMapper;
using NetBaires.Data;
using NetBaires.Data.Entities;

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