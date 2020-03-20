using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EFSecondLevelCache.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Helpers;
using NetBaires.Api.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.Community.GetCommunitySummary
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
            var sponsors = _context.Sponsors.Cacheable().AsNoTracking();
            var speakers = _context.Members
                                    .Where(x => x.Events.Any(s => s.Speaker))
                                    .Take(10)
                                    .Cacheable()
                                    .AsNoTracking();
            var lastEvents = _context.Events
                                     .OrderByDescending(x => x.Date)
                                     .Take(5)
                                     .Cacheable()
                                     .AsNoTracking();
            var organizers = _context.Members
                .Where(x => x.Organized)
                .Cacheable()
                .AsNoTracking();
            var eventsLive = await _context.Events
                .Where(x => x.Live)
                .Cacheable()
                .AnyAsync();
            var onlineEvent = await _context.Events
                .Where(x => x.Live
                            &&
                            x.Online)
                .Cacheable()
                .AnyAsync();

            var response = new GetCommunitySummaryQuery.Response
            {
                Sponsors = _mapper.Map<List<SponsorDetailViewModel>>(sponsors),
                Speakers = _mapper.Map<List<MemberDetailViewModel>>(speakers),
                Organizers = _mapper.Map<List<MemberDetailViewModel>>(organizers),
                LastEvents = _mapper.Map<List<EventDetailViewModel>>(lastEvents),
                TotalEvents = _context.Events.Count(),
                TotalUsersMeetup = _context.Members.Count(),
                TotalUsersSlack = 970,
                TotalSpeakers = _context.Members
                                    .Where(x => x.Events.Any(s => s.Speaker)).Count(),
                EventsLive = eventsLive,
                OnlineEvent = onlineEvent
            };
            return HttpResponseCodeHelper.Ok(response);
        }

    }
}