using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Badges.GetBadge
{
    public class AddMemberCommand : IRequest<IActionResult>
    {
        public string Email { get; set; } = string.Empty;
        public long? MeetupId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}