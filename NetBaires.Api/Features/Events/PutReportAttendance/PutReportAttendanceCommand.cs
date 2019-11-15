using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Handlers.Events
{
    public class PutReportAttendanceCommand : IRequest<IActionResult>
    {
        public string Token { get; set; }
    }
}