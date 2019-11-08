using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.NewBadge
{
    public class AssignBadgeToBadgeGroupHandler : IRequestHandler<AssignBadgeToBadgeGroupCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ILogger<AssignBadgeToBadgeGroupHandler> logger;

        public AssignBadgeToBadgeGroupHandler(NetBairesContext context,
            ILogger<AssignBadgeToBadgeGroupHandler> logger)
        {
            _context = context;
            logger = logger;
        }


        public async Task<IActionResult> Handle(AssignBadgeToBadgeGroupCommand request, CancellationToken cancellationToken)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == request.BadgeId);
            var badgeGroup = await _context.BadgeGroups.FirstOrDefaultAsync(x => x.Id == request.BadgeGroupId);
            if (badge == null || badgeGroup == null)
                return new StatusCodeResult(404);

            badgeGroup.Badges.Add(badge);
            await _context.SaveChangesAsync();
            return new StatusCodeResult(204);
        }
    }
}