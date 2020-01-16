using System.Collections.Generic;
using NetBaires.Api.ViewModels;

namespace NetBaires.Api.Features.Speakers.GetSpeakers
{
    public class GetSpeakerResponse 
    {
        public MemberDetailViewModel Member { get; set; }
        public int CountEventsAsSpeaker { get; set; }
        public List<EventDetailViewModel> Events { get; set; }

        public GetSpeakerResponse()
        {
            
        }
    }
}