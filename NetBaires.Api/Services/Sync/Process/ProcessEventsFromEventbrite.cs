using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Services.EventBrite;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Services.Sync.Process
{
    public class ProcessEventsFromEventbrite : IProcessEvents
    {
        private readonly IEventBriteServices _eventBriteServices;
        private readonly NetBairesContext _context;
        private readonly ISyncServices _syncServices;
        private readonly ILogger<ProcessEventsFromEventbrite> _logger;

        public ProcessEventsFromEventbrite(IEventBriteServices eventBriteServices,
            NetBairesContext context,
            ISyncServices syncServices,
            ILogger<ProcessEventsFromEventbrite> logger)
        {
            _eventBriteServices = eventBriteServices;
            _context = context;
            _syncServices = syncServices;
            _logger = logger;
        }
        public async Task Process()
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
    }
}