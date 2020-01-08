using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Events.UpdateAttendee
{
    public class UpdateAttendeeCommand : IRequest<IActionResult>
    {
        public string Status { get; set; }
        public bool? Organizer { get; set; }
        public bool? Speaker { get; set; }
        [JsonIgnore]
        public int EventId { get; internal set; }
        [JsonIgnore]
        public int MemberId { get; internal set; }
        public bool? Attended { get; set; }
        public bool? NotifiedAbsence { get; set; }
        public bool? DoNotKnow { get; set; }
    }
}