using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Services.EventBrite;
using NetBaires.Api.Services.Meetup;
using NetBaires.Api.Services.Sync;
using NetBaires.Data;

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
    public class SyncWithExternalEventsHandler : IRequestHandler<SyncWithExternalEventsHandler.SyncWithExternalEvents, IActionResult>
    {
        private readonly IMeetupServices _meetupServices;
        private readonly IEventBriteServices _eventBriteServices;
        private readonly NetBairesContext _context;
        private readonly ISyncServices _syncServices;
        private readonly ILogger<SyncWithExternalEventsHandler> _logger;

        public SyncWithExternalEventsHandler(IMeetupServices meetupServices,
            IEventBriteServices eventBriteServices,
            NetBairesContext context,
            ISyncServices syncServices,
            ILogger<SyncWithExternalEventsHandler> logger)
        {
            _meetupServices = meetupServices;
            _eventBriteServices = eventBriteServices;
            _context = context;
            _syncServices = syncServices;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(SyncWithExternalEvents request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start Sync with external Platform");

            await ProcessMeetupEvents();

            await ProcessEventBriteEvents();

            _logger.LogInformation($"End Sync with external Platform");

            return new StatusCodeResult(200);
        }

        private async Task ProcessEventBriteEvents()
        {
            var eventBriteEvents = await _eventBriteServices.GetEvents();
            var minesFromEventBrite = _context.Events.Where(x => x.Platform == EventPlatform.EventBrite).Select(x => x.EventId)
                .ToList();

            var eventEventBriteProcesseds = eventBriteEvents.Aggregate(new List<Event>(), (list, @event) =>
            {
                if (!minesFromEventBrite.Any(x => x == @event.Id))
                    list.Add(new Event
                    {
                        Description = @event.Description.Text,
                        Title = @event.Name.Text,
                        Url = @event.Url,
                        EventId = @event.Id.ToString(),
                        Date = @event.Start.Utc.Date,
                        ImageUrl = @event.Logo?.Original?.Url?.OriginalString,
                        Platform = EventPlatform.EventBrite
                    });
                return list;
            });
            _context.Events.AddRange(eventEventBriteProcesseds);

            await _context.SaveChangesAsync();

            foreach (var eventBriteProcessed in eventEventBriteProcesseds)
                await _syncServices.SyncEvent(eventBriteProcessed.Id);
        }

        private async Task ProcessMeetupEvents()
        {
            var eventsToAdd = await _meetupServices.GetAllEvents();
            var mines = _context.Events.Where(x => x.Platform == EventPlatform.Meetup).Select(x => x.EventId).ToList();
            var eventMeetupProcesseds = new List<Event>();
            foreach (var eventToAdd in eventsToAdd)
            {
                if (!mines.Any(x => x == eventToAdd.Id.ToString()))
                {
                    eventMeetupProcesseds.Add(new Event
                    {
                        Description = eventToAdd.Description,
                        Title = eventToAdd.Name,
                        Url = eventToAdd.Link.AbsoluteUri,
                        EventId = eventToAdd.Id.ToString(),
                        Date = eventToAdd.LocalDate.Date,
                        ImageUrl = eventToAdd?.FeaturedPhoto?.HighresLink?.AbsoluteUri,
                        Platform = EventPlatform.Meetup
                    });
                    _logger.LogInformation($"Created new event from Meetup : {eventToAdd.Id.ToString()}");
                }
            }

            if (eventMeetupProcesseds.Any())
                _context.Events.AddRange(eventMeetupProcesseds);
            await _context.SaveChangesAsync();

            foreach (var eventMeetupProcessed in eventMeetupProcesseds)
                await _syncServices.SyncEvent(eventMeetupProcessed.Id);
        }


        public class SyncWithExternalEvents : IRequest<IActionResult> { }

    }
}