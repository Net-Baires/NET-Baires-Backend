using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Helpers;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Badges.AssignMembersToBadge
{
    public class AssignMemberToBadgeHandler : IRequestHandler<AssignMemberToBadgeCommand, IActionResult>
    {
        private readonly NetBairesContext _context;

        public AssignMemberToBadgeHandler(NetBairesContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Handle(AssignMemberToBadgeCommand request, CancellationToken cancellationToken)
        {
            var membersAlreadyHasTheBadge =
                 _context.BadgeMembers.Any(x => x.BadgeId == request.BadgeId && x.MemberId == request.MemberId);
            if (membersAlreadyHasTheBadge)
                return new ObjectResult("El miembro que esta intentando asignar ya tiene ese Badge") { StatusCode = 409 }; ;

            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == request.BadgeId);
            if (badge == null)
                return HttpResponseCodeHelper.NotFound("No se encontro el Badge");
            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == request.MemberId
                                                                                    &&
                                                                                    !x.Badges.Any(b => b.BadgeId == request.BadgeId));
            if (member == null)
                return HttpResponseCodeHelper.NotFound("No se encontro el miembro");

            _context.BadgeMembers.Add(new BadgeMember
            {
                BadgeId = badge.Id,
                MemberId = member.Id
            });

            await _context.SaveChangesAsync();
            return new StatusCodeResult(204);
        }
    }
}