using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Services.Sync.Process;

namespace NetBaires.Api.Handlers.Events
{
    public class SyncWithExternalEventsHandler : IRequestHandler<SyncWithExternalEventsHandler.SyncWithExternalEvents, IActionResult>
    {
        private readonly IEnumerable<IProcessEvents> _processEvents;
        private readonly ILogger<SyncWithExternalEventsHandler> _logger;

        public SyncWithExternalEventsHandler(
            IEnumerable<IProcessEvents> processEvents,
            ILogger<SyncWithExternalEventsHandler> logger)
        {
            _processEvents = processEvents;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(SyncWithExternalEvents request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start Sync with external Platform");

            foreach (var processEvents in _processEvents)
                await processEvents.Process();

            _logger.LogInformation($"End Sync with external Platform");

            return new StatusCodeResult(204);
        }

        public class SyncWithExternalEvents : IRequest<IActionResult> { }

    }
}