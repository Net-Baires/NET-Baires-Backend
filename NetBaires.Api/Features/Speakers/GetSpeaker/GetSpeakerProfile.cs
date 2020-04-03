using AutoMapper;

namespace NetBaires.Api.Features.Speakers.GetSpeaker
{
    public class GetSpeakerProfile : Profile
    {
        public GetSpeakerProfile()
        {
            CreateMap<GetSpeakerHandler.MemberEvents, GetSpeakerResponse>();
        }
    }
}