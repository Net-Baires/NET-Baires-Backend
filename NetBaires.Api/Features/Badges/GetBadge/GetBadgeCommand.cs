using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Badges.GetBadge
{
    public class GetBadgeCommand : IRequest<IActionResult>
    {
        public GetBadgeCommand(int badgeId)
        {
            BadgeId = badgeId;
        }

        public int BadgeId { get; }
    }
}