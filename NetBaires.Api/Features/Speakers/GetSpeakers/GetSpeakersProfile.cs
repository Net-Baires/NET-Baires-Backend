using AutoMapper;
using NetBaires.Data;
using static NetBaires.Api.Handlers.Speakers.GetSpeakersHandler;

namespace NetBaires.Api.Handlers.Speakers
{
    public class GetSpeakersProfile : Profile
    {
        public GetSpeakersProfile()
        {
            CreateMap<Member, GetSpeakersResponse>();
        }
    }
}