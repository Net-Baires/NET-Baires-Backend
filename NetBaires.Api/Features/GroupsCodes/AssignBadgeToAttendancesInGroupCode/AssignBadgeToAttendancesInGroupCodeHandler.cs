using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.AssignBadgeToAttendances
{

    public class AssignBadgeToAttendancesInGroupCodeHandler : IRequestHandler<AssignBadgeToAttendancesInGroupCodeCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ILogger<AssignBadgeToAttendancesHandler> _logger;

        public AssignBadgeToAttendancesInGroupCodeHandler(NetBairesContext context,
            ILogger<AssignBadgeToAttendancesHandler> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(AssignBadgeToAttendancesInGroupCodeCommand request, CancellationToken cancellationToken)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == request.BadgeId);
            if (badge == null)
                return HttpResponseCodeHelper.NotFound("El badge que esta intentando asignar no se encuentra en el sistema");

            var groupCode = await _context.GroupCodes.Include(s => s.Members)
                                                         .Where(x => x.Id == request.GroupCodeId)
                                                         .FirstOrDefaultAsync();
            if (groupCode == null)
                return HttpResponseCodeHelper.NotFound("El badge que esta intentando asignar no se encuentra en el sistema");

            var response = groupCode.AssignBadge(badge);

            if (!response.SuccessResult)
                return HttpResponseCodeHelper.Error(response.Errors.First());

            await _context.SaveChangesAsync();

            return HttpResponseCodeHelper.NotContent();
        }
    }
}