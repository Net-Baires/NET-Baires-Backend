using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Features.Events.UpdateEvent;
using NetBaires.Api.Helpers;
using NetBaires.Api.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.Speakers.GetSpeakers
{

    public class GetSpeakerHandler : IRequestHandler<GetSpeakerQuery, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;

        public GetSpeakerHandler(IMapper mapper,
            NetBairesContext context,
            ILogger<UpdateEventHandler> logger)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<IActionResult> Handle(GetSpeakerQuery request, CancellationToken cancellationToken)
        {

            var eventToReturn = await _context.Members
                                        .Where(x =>
                                             x.Id == request.Id
                                            &&
                                            x.Events.Any(s => s.Speaker))
                                        .Select(x => new MemberEvents
                                        {
                                            Member = x,
                                            Events = _context.Events.Where(e=> e.Attendees.Any(a=> a.MemberId == x.Id && a.Speaker)).ToList(),
                                            CountEventsAsSpeaker = x.Events.Count(s => s.Speaker)
                                        })
                                        .FirstOrDefaultAsync();
            if (eventToReturn == null)
                return HttpResponseCodeHelper.NotContent();



            return HttpResponseCodeHelper.Ok(_mapper.Map<GetSpeakerResponse>(eventToReturn));
        }
        public class MemberEvents
        {
            public Member Member { get; set; }
            public List<Event> Events { get; set; }
            public int CountEventsAsSpeaker { get; set; }
        }
    }
}