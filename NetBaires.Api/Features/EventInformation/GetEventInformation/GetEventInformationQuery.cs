using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.EventInformation.GetEventInformation
{
    public class GetEventInformationQuery : IRequest<IActionResult>
    {
        public GetEventInformationQuery(int eventId)
        {
            EventId = eventId;
        }

        public int EventId { get;  }
    }
}