using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Features.Events.ViewModels;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class GetAttendeesHandler : IRequestHandler<GetAttendeesQuery, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<GetAttendeesHandler> _logger;

        public GetAttendeesHandler(IMapper mapper,
            NetBairesContext context,
            ILogger<GetAttendeesHandler> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetAttendeesQuery request, CancellationToken cancellationToken)
        {
            var attendees = new List<Attendance>();
            if (request.MemberId != null)
                attendees = await _context.Attendances
                                           .Include(x => x.Member)
                                           .Where(x => x.EventId == request.EventId
                                                       &&
                                                       request.MemberId.Value == x.MemberId)
                                           .ToListAsync();
            else attendees = await _context.Attendances
                            .Include(x => x.Member)
                            .Where(x => x.EventId == request.EventId)
                            .ToListAsync();

            if (attendees == null || !attendees.Any())
                return HttpResponseCodeHelper.NotContent();

            if (attendees.Count == 1)
                return HttpResponseCodeHelper.Ok(_mapper.Map(attendees, new List<AttendantViewModel>()).First());
            else
                return HttpResponseCodeHelper.Ok(_mapper.Map(attendees, new List<AttendantViewModel>()));
        }

    }
}