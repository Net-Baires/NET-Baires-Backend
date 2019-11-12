using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Badges.AssignMembersToBadge
{
    public class GetBadgesFromEmailQuery : IRequest<IActionResult>
    {
        public string Email { get; set; }
    }
}