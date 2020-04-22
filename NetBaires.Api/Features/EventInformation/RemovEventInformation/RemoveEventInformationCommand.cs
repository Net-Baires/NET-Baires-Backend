using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.EventInformation.RemovEventInformation
{
    public class RemoveEventInformationCommand : IRequest<IActionResult>
    {
        public int EventId { get; set; }
        public int EventInformationId { get; set; }

        public RemoveEventInformationCommand(int eventId, int eventInformationId)
        {
            EventId = eventId;
            EventInformationId = eventInformationId;
        }
    }
}