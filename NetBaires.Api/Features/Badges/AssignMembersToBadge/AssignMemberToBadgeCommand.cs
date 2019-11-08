using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Badges.AssignMembersToBadge
{
    public class AssignMemberToBadgeCommand : IRequest<IActionResult>
    {
        public int BadgeId { get; set; }
        public int MemberId { get; set; }

        public AssignMemberToBadgeCommand(int badgeId, int memberId)
        {
            BadgeId = badgeId;
            MemberId = memberId;
        }
        public AssignMemberToBadgeCommand()
        {

        }
    }
}