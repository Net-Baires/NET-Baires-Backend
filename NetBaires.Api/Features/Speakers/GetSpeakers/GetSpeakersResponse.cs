using NetBaires.Api.Features.Members.ViewModels;

namespace NetBaires.Api.Handlers.Speakers
{
    public class GetSpeakersResponse : MemberDetailViewModel
    {
        public int CounEventsAsSpeaker { get; set; }
    }
}