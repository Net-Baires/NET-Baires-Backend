using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Badges.GetBadge
{
    public class GetBadgeQuery : IRequest<IActionResult>
    {
        public GetBadgeQuery(int? badgeId)
        {
            BadgeId = badgeId;
        }
        public GetBadgeQuery()
        {

        }
        public int? BadgeId { get; }
    }
}