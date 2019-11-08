using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Badges.NewBadge
{
    public class AssignBadgeToBadgeGroupCommand : IRequest<IActionResult>
    {

        public int BadgeGroupId { get; set; }
        public int BadgeId { get; set; }
    }
}