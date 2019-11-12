using System.Collections.Generic;
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
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class GetLivesHandler : IRequestHandler<GetLivesQuery, IActionResult>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<UpdateEventHandler> _logger;

        public GetLivesHandler(ICurrentUser currentUser,
            IMapper mapper,
            NetBairesContext context,
            IOptions<AttendanceOptions> assistanceOptions,
            ILogger<UpdateEventHandler> logger)
        {
            _currentUser = currentUser;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(GetLivesQuery request, CancellationToken cancellationToken)
        {
            var eventToReturn = _context.Events.Include(i => i.Attendees).Where(x => x.Live)
                .ToList()
                .Select(s => new SelectEventLive(s, _context.Attendances.Any(a => a.MemberId == _currentUser.User.Id
                                                                                  &&
                                                                                  a.EventId == s.Id))).ToList();
            if (eventToReturn == null)
                return new StatusCodeResult(204);

            var returnValues = _mapper.Map(eventToReturn, new List<GetLivesResponse>());

            return new ObjectResult(returnValues) { StatusCode = 200 };
        }
    }
}