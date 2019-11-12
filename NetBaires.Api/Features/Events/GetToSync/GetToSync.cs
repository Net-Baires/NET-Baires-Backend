using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Models;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class GetToSyncHandler : IRequestHandler<GetToSyncQuery, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetToSyncHandler> _logger;

        public GetToSyncHandler(NetBairesContext context,
            IMapper mapper,
            ILogger<GetToSyncHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetToSyncQuery request, CancellationToken cancellationToken)
        {
            var newEvents = await _context.Events.Include(x => x.Attendees).OrderByDescending(x => x.Id).Where(x => !x.Done)
                .ToListAsync();
            var eventToReturn = newEvents?
                .Select(x => new GetToSyncResponse(x,
                    x.Attendees.Count(s => s.Attended),
                    x.Attendees.Count(s => s.DidNotAttend),
                    x.Attendees.Count()))?.ToList();

            if (!eventToReturn.Any())
                return new StatusCodeResult(204);

            return new ObjectResult(eventToReturn) { StatusCode = 200 };

        }
    }
}