using AutoMapper;

namespace NetBaires.Api.Features.Speakers.GetSpeakers
{
    public class GetSpeakerProfile : Profile
    {
        public GetSpeakerProfile()
        {
            CreateMap<GetSpeakerHandler.MemberEvents, GetSpeakerResponse>();
        }
    }
}