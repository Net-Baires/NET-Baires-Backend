using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.GroupsCodes.AssignBadgeToAttendancesInGroupCode
{
    public class AssignBadgeToAttendancesInGroupCodeCommand : IRequest<IActionResult>
    {
        public int GroupCodeId { get; set; }
        public int BadgeId { get; set; }
    }
}