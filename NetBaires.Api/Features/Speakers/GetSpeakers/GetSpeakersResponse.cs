using NetBaires.Api.ViewModels;

namespace NetBaires.Api.Features.Speakers.GetSpeakers
{
    public class GetSpeakersResponse : MemberDetailViewModel
    {
        public int CounEventsAsSpeaker { get; set; }
    }
}