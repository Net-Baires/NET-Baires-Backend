using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EFSecondLevelCache.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Helpers;
using NetBaires.Api.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.GetAttendees
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
            var query = _context.Attendances
                .Include(x => x.Member)
                .ThenInclude(s => s.Events)
                .Where(x => x.EventId == request.EventId);
            
            if (request.MemberId != null)
                query = query.Where(x => request.MemberId.Value == x.MemberId);

            if (!string.IsNullOrWhiteSpace(request.Query))
                query = query.Where(x => x.Member.Email.Contains(request.Query)
                                         ||
                                         x.Member.FirstName.Contains(request.Query)
                                         ||
                                         x.Member.LastName.Contains(request.Query)
                                         ||
                                         x.Member.MeetupId.ToString().Contains(request.Query));

            var attendees = await query.Select(s => _mapper.Map<AttendantViewModel>(s))
                .ToListAsync(cancellationToken: cancellationToken);



            if (attendees == null || !attendees.Any())
                return HttpResponseCodeHelper.NotContent();

            if (attendees.Count == 1 && request.MemberId != null)
                return HttpResponseCodeHelper.Ok(attendees.First());
            return HttpResponseCodeHelper.Ok(attendees);
        }

    }
}