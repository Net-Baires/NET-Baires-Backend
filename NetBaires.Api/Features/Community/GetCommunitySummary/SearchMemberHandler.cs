using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Events.ViewModels;
using NetBaires.Api.Features.Members.ViewModels;
using NetBaires.Api.Handlers.Sponsors;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.GetBadge
{

    public class GetCommunitySummaryHandler : IRequestHandler<GetCommunitySummaryQuery, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;

        public GetCommunitySummaryHandler(
            NetBairesContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(GetCommunitySummaryQuery request, CancellationToken cancellationToken)
        {
            var sponsors = _context.Sponsors.AsNoTracking();
            var speakers = _context.Members
                                    .Where(x => x.Events.Any(s => s.Speaker))
                                    .Take(10)
                                    .AsNoTracking();
            var lastEvents = _context.Events
                                     .OrderByDescending(x => x.Date)
                                     .Take(5)
                                     .AsNoTracking();
            var organizers = _context.Members
                                    .Where(x => x.Organized)
                                    .AsNoTracking();
            var response = new GetCommunitySummaryQuery.Response
            {
                Sponsors = _mapper.Map<List<SponsorDetailViewModel>>(sponsors),
                Speakers = _mapper.Map<List<MemberDetailViewModel>>(speakers),
                Organizers = _mapper.Map<List<MemberDetailViewModel>>(organizers),
                LastEvents = _mapper.Map<List<EventDetailViewModel>>(lastEvents),
                TotalEvents = _context.Events.Count(),
                TotalUsersMeetup = _context.Members.Count(),
                TotalSpeakers = _context.Members
                                    .Where(x => x.Events.Any(s => s.Speaker)).Count()
            };
            return HttpResponseCodeHelper.Ok(response);
        }

    }
}