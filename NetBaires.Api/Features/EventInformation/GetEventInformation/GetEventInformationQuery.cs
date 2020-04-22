using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.EventInformation.GetEventInformation
{
    public class GetEventInformationQuery : IRequest<IActionResult>
    {
        public GetEventInformationQuery(int eventId, bool? visible)
        {
            EventId = eventId;
            Visible = visible;
        }

        public int EventId { get; }
        public bool? Visible { get; set; } = null;
    }
}