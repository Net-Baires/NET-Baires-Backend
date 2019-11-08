using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Handlers.Badges.UpdateBadge;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Badges
{
    public class AssignMemberToBadgeHandler : IRequestHandler<AssignMemberToBadgeHandler.AssignMemberToBadge, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly IBadgesServices badgesServices;
        private readonly ILogger<GetToAssignHandler> _logger;

        public AssignMemberToBadgeHandler(NetBairesContext context,
            IMapper mapper,
            IBadgesServices badgesServices,
            ILogger<UpdateBadgeHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            this.badgesServices = badgesServices;
        }


        public async Task<IActionResult> Handle(AssignMemberToBadge request, CancellationToken cancellationToken)
        {
            var membersAlreadyHasTheBadge =
                 _context.BadgeMembers.Any(x => x.BadgeId == request.BadgeId && x.MemberId == request.MemberId);
            if (membersAlreadyHasTheBadge)
                return new ObjectResult("El miembro que esta intentando asignar ya tiene ese Badge") { StatusCode = 409 }; ;

            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == request.BadgeId);
            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == request.MemberId);

            _context.BadgeMembers.Add(new BadgeMember
            {
                BadgeId = badge.Id,
                MemberId = member.Id
            });

            await _context.SaveChangesAsync();
            return new StatusCodeResult(204);

        }


        public class AssignMemberToBadge : IRequest<IActionResult>
        {
            public int BadgeId { get; set; }
            public int MemberId { get; set; }

            public AssignMemberToBadge(int badgeId, int memberId)
            {
                BadgeId = badgeId;
                MemberId = memberId;
            }
        }
    }
}