using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NetBaires.Api.Features.Events.GetAttendees
{
    public class GetAttendeesQuery : IRequest<IActionResult>
    {
        [JsonIgnore]
        public int EventId { get; set; }
        public int? MemberId { get; set; }
        public string Query { get; set; }
        public GetAttendeesQuery(int eventId, int? memberId, string query)
        {
            this.EventId = eventId;
            this.MemberId = memberId;
            Query = query;
        }
    }
}