using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Features.Speakers.GetSpeakers
{
    public class GetSpeakersProfile : Profile
    {
        public GetSpeakersProfile()
        {
            CreateMap<Member, GetSpeakersResponse>();
        }
    }
}