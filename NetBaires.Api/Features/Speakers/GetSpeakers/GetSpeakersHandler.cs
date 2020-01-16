using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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
                                        .Where(x =>
                                            request.Id != null ? x.Id == request.Id : true
                                            &&
                                            x.Events.Any(s => s.Speaker))
                                        .ToList()
                                        .Select(x => new GetSpeakerResponse
                                        {
                                            Member = _mapper.Map<MemberDetailViewModel>(x),
                                            CountEventsAsSpeaker = x.Events.Count(s => s.Speaker)
                                        })
                                        .ToList();
     

            if (!eventToReturn.Any())
                return HttpResponseCodeHelper.NotContent();

            if (request.Id != null)
                return HttpResponseCodeHelper.Ok(eventToReturn.First());
            return HttpResponseCodeHelper.Ok(eventToReturn);
        }
        public class MemberEvents
        {
            public Member Member { get; set; }
            public int CountEventsAsSpeaker { get; set; }
        }
    }
}