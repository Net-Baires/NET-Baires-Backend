using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Badges.GetMembersInBadge
{
    public class GetMembersInBadgeQuery : IRequest<IActionResult>
    {
        public int BadgeId { get; set; }

        public GetMembersInBadgeQuery(int badgeId)
        {
            BadgeId = badgeId;
        }
        public GetMembersInBadgeQuery()
        {

        }
    }
}