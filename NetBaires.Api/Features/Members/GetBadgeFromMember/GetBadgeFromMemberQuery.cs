using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Members.GetBadgeFromMember
{
    public class GetBadgeFromMemberQuery : IRequest<IActionResult>
    {
        public int Id { get; set; }
        public int BadgeId { get; set; }

    }
}