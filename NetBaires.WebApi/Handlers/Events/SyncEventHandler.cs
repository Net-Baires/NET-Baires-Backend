using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Services.Sync;

namespace NetBaires.Api.Handlers.Events
{
    public class SyncEventHandler : IRequestHandler<SyncEventHandler.SyncEvent, IActionResult>
    {
        private readonly ISyncServices _syncServices;
        private readonly ILogger<SyncEventHandler> _logger;

        public SyncEventHandler(ISyncServices syncServices,
            ILogger<SyncEventHandler> logger)
        {
            _syncServices = syncServices;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(SyncEvent request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start Sync Event {request.EventId} with external Platform");

            await _syncServices.SyncEvent(request.EventId);

            _logger.LogInformation($"End Sync Event {request.EventId} with external Platform");

            return new StatusCodeResult(204);
        }


        public class SyncEvent : IRequest<IActionResult>
        {
            public SyncEvent(int eventId)
            {
                EventId = eventId;
            }

            public int EventId { get; }
        }

    }
}