using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Events.PutCheckAttendanceByCode
{
    public class PutCheckAttendanceByCodeCommand : IRequest<IActionResult>
    {
        public int EventId { get; set; }
        public string Code { get; set; }

        public PutCheckAttendanceByCodeCommand(int eventId, string code)
        {
            EventId = eventId;
            Code = code;
        }
    }
}