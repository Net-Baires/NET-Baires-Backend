using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Badges.GetBadge
{
    public class AddMemberCommand : IRequest<IActionResult>
    {
        public string Email { get; set; }
        public long? MeetupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}