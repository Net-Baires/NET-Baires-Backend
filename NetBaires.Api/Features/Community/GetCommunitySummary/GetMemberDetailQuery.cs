using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.ViewModels;

namespace NetBaires.Api.Features.Community.GetCommunitySummary
{
    public class GetCommunitySummaryQuery : IRequest<IActionResult>
    {

        public class Response
        {
            public List<MemberDetailViewModel> Organizers { get; set; }
            public List<MemberDetailViewModel> Speakers { get; set; }
            public List<EventDetailViewModel> LastEvents { get; set; }
            public List<SponsorDetailViewModel> Sponsors { get; set; }
            public int TotalEvents { get; set; }
            public int TotalUsersMeetup { get; set; }
            public int TotalSpeakers { get; set; }
            public int TotalUsersSlack { get; set; }
        }
    }
}