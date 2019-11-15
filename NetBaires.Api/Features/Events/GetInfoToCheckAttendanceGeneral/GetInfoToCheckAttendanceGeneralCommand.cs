using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Handlers.Events
{
    public class GetInfoToCheckAttendanceGeneralCommand : IRequest<IActionResult>
    {
        public int Id { get; set; }
    }
}