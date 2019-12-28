using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Handlers.Events
{
    public class GetAttendeesQuery : IRequest<IActionResult>
    {
        public int EventId { get; set; }
        public int? MemberId { get; set; }
        public GetAttendeesQuery(int eventId, int? memberId)
        {
            this.EventId = eventId;
            this.MemberId = memberId;
        }

    

    }
}