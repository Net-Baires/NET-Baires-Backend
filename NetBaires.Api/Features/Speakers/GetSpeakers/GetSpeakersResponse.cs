using NetBaires.Api.ViewModels;

namespace NetBaires.Api.Features.Speakers.GetSpeakers
{
    public class GetSpeakersResponse
    {
        public MemberDetailViewModel Member { get; set; }
        public int CountEventsAsSpeaker { get; set; }

        public GetSpeakersResponse()
        {
            
        }
    }
}