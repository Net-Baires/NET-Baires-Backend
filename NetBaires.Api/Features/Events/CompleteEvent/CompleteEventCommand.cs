using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Events.CompleteEvent
{
    public class CompleteEventCommand :  Data.Entities.CompleteEvent, IRequest<IActionResult>
    {
        public int Id { get; set; }
    }
}