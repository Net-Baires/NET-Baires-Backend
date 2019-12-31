using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Handlers.Events
{
    public class PutCheckAttendanceGeneralCommand : IRequest<IActionResult>
    {
        public string Token { get; set; }
    }
}