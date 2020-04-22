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
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Events.GetSpeakersInEvent
{

    public class GetSpeakersInEventHandler : IRequestHandler<GetSpeakersInEventQuery, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;

        public GetSpeakersInEventHandler(IMapper mapper,
            NetBairesContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<IActionResult> Handle(GetSpeakersInEventQuery request, CancellationToken cancellationToken)
        {

            var speakers = _context.Attendances.Include(x=> x.Member).Where(x => x.EventId == request.Id
                                                                                          &&
                                                                                          x.Speaker)
                                        .Cacheable()
                                        .ToList()
                                        .Select(x => new GetSpeakersInEventInEventResponse
                                        {
                                            Member = _mapper.Map<MemberDetailViewModel>(x.Member)
                                        })
                                        .ToList();

            if (!speakers.Any())
                return HttpResponseCodeHelper.NotContent();

            return HttpResponseCodeHelper.Ok(speakers);
        }
        public class MemberEvents
        {
            public Member Member { get; set; }
            public int CountEventsAsSpeaker { get; set; }
        }
    }
}