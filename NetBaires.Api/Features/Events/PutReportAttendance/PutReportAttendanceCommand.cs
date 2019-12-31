using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Events.PutReportAttendance
{
    public class PutReportAttendanceCommand : IRequest<IActionResult>
    {
        public string Token { get; set; }

        public class Response
        {
            public int EventId { get; set; }
            public int MemberId { get; }

            public Response(int eventId, int memberId)
            {
                EventId = eventId;
                MemberId = memberId;
            }

    
        }
    }
}