using AutoMapper;
using NetBaires.Data;
using NetBaires.Data.Entities;

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