using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Handlers.Events;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Speakers
{

    public class GetSpeakersHandler : IRequestHandler<GetSpeakersQuery, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;

        public GetSpeakersHandler(IMapper mapper,
            NetBairesContext context,
            ILogger<UpdateEventHandler> logger)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<IActionResult> Handle(GetSpeakersQuery request, CancellationToken cancellationToken)
        {

            var eventToReturn = _context.Members
                                        .Include(x => x.Events)
                                        .Where(x => x.Events.Any(s => s.Speaker))
                                        .ToList()
                                        .Select(x => new MemberEvents
                                        {
                                            Member = x,
                                            CountEventsAsSpeaker = x.Events.Count(s => s.Speaker)
                                        })
                                        .ToList();
            var returnList = new List<GetSpeakersResponse>();

            foreach (var item in eventToReturn)
            {
                var itemToAdd = _mapper.Map<GetSpeakersResponse>(item.Member);
                itemToAdd.CounEventsAsSpeaker = item.CountEventsAsSpeaker;
                returnList.Add(itemToAdd);
            }


            if (!eventToReturn.Any())
                return HttpResponseCodeHelper.NotContent();

            return HttpResponseCodeHelper.Ok(returnList);
        }
        public class MemberEvents
        {
            public Member Member { get; set; }
            public int CountEventsAsSpeaker { get; set; }
        }
    }
}