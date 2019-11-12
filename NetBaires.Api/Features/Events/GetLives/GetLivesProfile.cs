using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{
    public class GetLivesProfile : Profile
    {
        public GetLivesProfile()
        {
            CreateMap<Event, GetLivesResponse>();

            CreateMap<SelectEventLive, GetLivesResponse>()
                .ForMember(dest => dest.Registered, opt => opt.MapFrom(f => f.Registered));
        }
    }
}