using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Members.InformAttendances
{
    public class InformAttendancesCommand : IRequest<IActionResult>
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public bool Attended { get; set; }
    }
}