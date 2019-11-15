using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Handlers.Events
{
    public class GetDataToReportAttendanceToEventCommand : IRequest<IActionResult>
    {
        public int Id { get; set; }
    }
}