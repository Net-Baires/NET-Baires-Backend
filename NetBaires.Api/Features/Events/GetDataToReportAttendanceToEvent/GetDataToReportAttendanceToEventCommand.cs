using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Events.GetDataToReportAttendanceToEvent
{
    public class GetDataToReportAttendanceToEventCommand : IRequest<IActionResult>
    {
        public int Id { get; set; }
    }
}