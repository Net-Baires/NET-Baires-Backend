using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Helpers;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Events.CompleteEvent
{

    public class CompleteEventHandler : IRequestHandler<CompleteEventCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ILogger<CompleteEventHandler> _logger;

        public CompleteEventHandler(
            NetBairesContext context,
            ILogger<CompleteEventHandler> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(CompleteEventCommand request, CancellationToken cancellationToken)
        {
            var eventToUpdate = await _context.Events.Include(x => x.Attendees)
                .ThenInclude(x => x.Member)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (eventToUpdate == null)
                return HttpResponseCodeHelper.NotFound();

            Badge badge = default;
            if (request.GiveBadgeToAttendees)
                badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == request.BadgeId, cancellationToken: cancellationToken);

            eventToUpdate.Complete(request, badge);

            await _context.SaveChangesAsync(cancellationToken);

            return HttpResponseCodeHelper.NotContent();
        }
    }
}