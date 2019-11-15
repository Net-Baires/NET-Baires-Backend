﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Features.Events.ViewModels;
using NetBaires.Api.Helpers;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
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
                                                     .Where(x => (request.Done != null ? x.Done == request.Done : true)
                                                            &&
                                                            (request.Live != null ? x.Live == request.Live : true)
                                                            &&
                                                            (request.Id != null ? x.Id == request.Id : true))
                                               .OrderByDescending(x => x.Id)
                                               .Select(x => _mapper.Map<Event, EventDetailViewModel>(x))
                                               .AsNoTracking()
                                               .ToListAsync();
            if (!eventToReturn.Any())
                return HttpResponseCodeHelper.NotContent();

            if (request.Id != null)
                return HttpResponseCodeHelper.Ok(eventToReturn.First());
            return HttpResponseCodeHelper.Ok(eventToReturn);
        }
    }

}