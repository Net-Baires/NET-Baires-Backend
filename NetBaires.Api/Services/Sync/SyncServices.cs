using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Options;
using NetBaires.Api.Services.EventBrite;
using NetBaires.Api.Services.Meetup;
using NetBaires.Data;
using Member = NetBaires.Data.Member;

namespace NetBaires.Api.Services.Sync
{
    public class SyncServices : ISyncServices
    {
        private readonly IMeetupServices _meetupServices;
        private readonly IEventBriteServices _eventBriteServices;
        private readonly NetBairesContext _context;
        private readonly ILogger<SyncServices> _logger;

        public SyncServices(IMeetupServices meetupServices,
            IEventBriteServices eventBriteServices,
            NetBairesContext context,
            ILogger<SyncServices> logger)
        {
            _meetupServices = meetupServices;
            _eventBriteServices = eventBriteServices;
            _context = context;
            _logger = logger;
        }
        public async Task SyncEvent(int internalEventId)
        {
            var eventToSync = _context.Events
                .Include(x => x.Attendees)
                .ThenInclude(x => x.Member).FirstOrDefault(x => x.Id == internalEventId);

            if (eventToSync.Platform == EventPlatform.Meetup)
                await ProcessAttendeesFromMeetup(eventToSync);


            if (eventToSync.Platform == EventPlatform.EventBrite)
                await ProcessAttendeesFromEventBrite(eventToSync);
            await _context.SaveChangesAsync();

        }
        private async Task ProcessAttendeesFromEventBrite(Event eventToSync)
        {
            var meetupAttendees = await _eventBriteServices.GetAttendees(eventToSync.EventId);
            foreach (var attendees in meetupAttendees)
            {
                var currentMember = eventToSync.Attendees.FirstOrDefault(x => x.Member.Email == attendees.profile.Email);
                if (currentMember == null)
                {
                    var newMember = new Member
                    {
                        FirstName = attendees.profile.FirstName,
                        LastName = attendees.profile.LastName,
                        Email = attendees.profile.Email,
                    };
                    if ((attendees.CheckIn))
                        currentMember = new EventMember(newMember, eventToSync, true);
                    else
                        currentMember = new EventMember(newMember, eventToSync, false);
                    await _context.EventMembers.AddAsync(currentMember);
                }
                else
                {
                    if ((attendees.CheckIn))
                        currentMember.Attend();
                }
            }
        }
        private async Task ProcessAttendeesFromMeetup(Event eventToSync)
        {
            var meetupAttendees = await _meetupServices.GetAttendees(int.Parse(eventToSync.EventId));
            var meetupAttendeesIds = meetupAttendees.Select(s => s.Member.Id);
            var attendeesToEach = await _context.EventMembers.Where(x => meetupAttendeesIds.Contains(x.Member.MeetupId)).ToListAsync();
            foreach (var attende in meetupAttendees)
            {
                var currentMember = attendeesToEach.FirstOrDefault(x => x.Member.MeetupId == attende.Member.Id);
                if (currentMember == null)
                {
                    if (attende.Member.Id != 0)
                    {
                        var newMember = new Member
                        {
                            MeetupId = attende.Member.Id,
                            FirstName = attende.Member.Name,
                            Picture = attende.Member.Photo?.HighresLink?.AbsolutePath == null ? "" :
                            attende.Member.Photo?.HighresLink?.AbsoluteUri,
                            Biography = attende.Member.Bio
                        };
                        if ((attende.Status != null
                             &&
                             attende.Status == "attended"))
                            currentMember = new EventMember(newMember, eventToSync, true);
                        else
                            currentMember = new EventMember(newMember, eventToSync);
                        await _context.EventMembers.AddAsync(currentMember);
                    }
                }
                else
                {
                    if ((attende.Status != null
                         &&
                         attende.Status == "attended"))
                        currentMember.Attend();
                }

            }
            await _context.SaveChangesAsync();
        }
    }
}