using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Events.SyncWithExternalEvents
{
    public class SyncWithExternalEventsCommand : IRequest<IActionResult> { }
}