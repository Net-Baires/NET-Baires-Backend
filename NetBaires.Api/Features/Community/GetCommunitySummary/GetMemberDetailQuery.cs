using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Features.Events.ViewModels;
using NetBaires.Api.Features.Members.ViewModels;
using NetBaires.Api.Handlers.Sponsors;
using System.Collections.Generic;

namespace NetBaires.Api.Features.Badges.GetBadge
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
            public int TotalUsersSlack { get; set; }
        }
    }
}