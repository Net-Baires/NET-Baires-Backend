using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.BadgeGroups.AssignBadgeToBadgeGroup
{
    public class AssignBadgeToBadgeGroupCommand : IRequest<IActionResult>
    {

        public int BadgeGroupId { get; set; }
        public int BadgeId { get; set; }
    }
}