using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Events.GetInfoToCheckAttendanceGeneral
{
    public class GetInfoToCheckAttendanceGeneralCommand : IRequest<IActionResult>
    {
        public int Id { get; set; }
    }
}