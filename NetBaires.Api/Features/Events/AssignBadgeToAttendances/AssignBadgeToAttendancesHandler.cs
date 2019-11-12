using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class AssignBadgeToAttendancesHandler : IRequestHandler<AssignBadgeToAttendancesCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ILogger<AssignBadgeToAttendancesHandler> _logger;

        public AssignBadgeToAttendancesHandler(NetBairesContext context,
            ILogger<AssignBadgeToAttendancesHandler> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(AssignBadgeToAttendancesCommand request, CancellationToken cancellationToken)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == request.BadgeId);
            if (badge == null)
                return HttpResponseCodeHelper.NotFound("El badge que esta intentando asignar no se encuentra en el sistema");

            var eventToAssign = await _context.Events.Include(x => x.Attendees)
                                                     .ThenInclude(x => x.Member)
                                                     .FirstOrDefaultAsync(x => x.Id == request.EventId);
            if (eventToAssign == null)
                return HttpResponseCodeHelper.NotFound("El badge que esta intentando asignar no se encuentra en el sistema");

            var response = eventToAssign.AssignBadgeToAttended(badge);

            if (!response.SuccessResult)
                return HttpResponseCodeHelper.Error(response.Errors.First());

            await _context.SaveChangesAsync();

            return HttpResponseCodeHelper.NotContent();
        }
    }
}