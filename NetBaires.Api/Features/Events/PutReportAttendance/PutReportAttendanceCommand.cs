using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Events.PutReportAttendance
{
    public class PutReportAttendanceCommand : IRequest<IActionResult>
    {
        public string Token { get; set; }
    }
}