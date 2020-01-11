using System.Collections.Generic;
using NetBaires.Api.ViewModels;

namespace NetBaires.Api.Features.Speakers.GetSpeakers
{
    public class GetSpeakerResponse : MemberDetailViewModel
    {
        public int CountEventsAsSpeaker { get; set; }
        public List<EventDetailViewModel> Events { get; set; }
    }
}