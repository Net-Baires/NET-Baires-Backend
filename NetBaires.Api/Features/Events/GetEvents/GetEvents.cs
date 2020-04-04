using System;
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

namespace NetBaires.Api.Features.Events.GetEvents
{

    public class GetEventsHandler : IRequestHandler<GetEventsQuery, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;

        public GetEventsHandler(IMapper mapper,
            NetBairesContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<IActionResult> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {

            var eventToReturn = await _context.Events.Include(x => x.Attendees)
                                                     .Include(x => x.Sponsors)
                                                     .Where(x => (request.Done != null ? x.Done == request.Done : true)
                                                            &&
                                                            (request.Live != null ? x.Live == request.Live : true)
                                                            &&
                                                            (request.Upcoming != null ? x.Date > DateTime.Now : true)
                                                            &&
                                                            (request.Id != null ? x.Id == request.Id : true))
                                                     .Cacheable()
                                               .ToListAsync(cancellationToken: cancellationToken);
            if (!eventToReturn.Any())
                return HttpResponseCodeHelper.NotContent();

            if (request.Id != null)
                return HttpResponseCodeHelper.Ok(_mapper.Map<EventDetailViewModel>(eventToReturn.First()));
            return HttpResponseCodeHelper.Ok(_mapper.Map<List<EventDetailViewModel>>(eventToReturn));
        }
    }

}