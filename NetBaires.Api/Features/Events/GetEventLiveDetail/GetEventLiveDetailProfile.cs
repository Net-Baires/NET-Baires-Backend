using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.GetEventLiveDetail
{
    public class GetEventLiveDetailProfile : Profile
    {
        public GetEventLiveDetailProfile()
        {
            CreateMap<Event, GetEventLiveDetailQuery.Response>()
                .ForMember(dest => dest.Attended, o => o.MapFrom<GetEventLiveDetailCurrentUserResolver>())
                .ForMember(dest => dest.HasGroupCodeOpen, o => o.MapFrom<HadGroupCodeOpenResolver>());
            CreateMap<Member, GetEventLiveDetailQuery.Response.MemberDetail>();

        }
    }
}
