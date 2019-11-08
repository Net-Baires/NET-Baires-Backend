using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Services.Meetup;
using NetBaires.Data;

namespace NetBaires.Api.Services.Sync.Process
{
    public class ProcessEventsFromMeetup : IProcessEvents
    {
        private readonly IMeetupServices _meetupServices;
        private readonly NetBairesContext _context;
        private readonly ISyncServices _syncServices;
        private readonly ILogger<ProcessEventsFromMeetup> _logger;

        public ProcessEventsFromMeetup(IMeetupServices meetupServices,
            NetBairesContext context,
            ISyncServices syncServices,
            ILogger<ProcessEventsFromMeetup> logger)
        {
            _meetupServices = meetupServices;
            _context = context;
            _syncServices = syncServices;
            _logger = logger;
        }
        public async Task Process()
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
    }
}