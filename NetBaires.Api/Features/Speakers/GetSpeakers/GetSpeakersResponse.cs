using System.Collections.Generic;
using NetBaires.Api.ViewModels;

namespace NetBaires.Api.Features.Speakers.GetSpeakers
{
    public class GetSpeakerResponse : MemberDetailViewModel
    {
        public int CountEventsAsSpeaker { get; set; }
        public List<AttendantViewModel> Events { get; set; }

        public GetSpeakerResponse()
        {
            
        }
    }
}