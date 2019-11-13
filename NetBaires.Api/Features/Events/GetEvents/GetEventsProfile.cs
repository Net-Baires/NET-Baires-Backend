using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{
    public class GetEventsProfile : Profile
    {
       
        public GetEventsProfile()
        {
            CreateMap<Event, GetEventsResponse>()
                .ForMember(dest => dest.Registered, o => o.MapFrom<RegisteredCurrentUserInEventResolver>());

        }
    }

}