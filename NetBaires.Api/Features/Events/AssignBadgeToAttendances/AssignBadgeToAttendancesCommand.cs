using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Handlers.Events
{
    public class AssignBadgeToAttendancesCommand : IRequest<IActionResult>
    {
        public int EventId { get; set; }
        public int BadgeId { get; set; }
    }
}