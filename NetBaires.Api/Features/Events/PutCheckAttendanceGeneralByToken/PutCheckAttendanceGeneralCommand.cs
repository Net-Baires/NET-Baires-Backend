using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Events.PutCheckAttendanceGeneralByToken
{
    public class PutCheckAttendanceGeneralCommand : IRequest<IActionResult>
    {
        public string Token { get; set; }
    }
}