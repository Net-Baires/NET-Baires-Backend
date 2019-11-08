using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Services.Meetup;
using NetBaires.Data;
using Member = NetBaires.Data.Member;

namespace NetBaires.Api.Services.Sync
{
    public class MeetupSyncServices : IExternalsSyncServices
    {
        private readonly IMeetupServices _meetupServices;
        private readonly NetBairesContext _context;
        private readonly ILogger<SyncServices> _logger;

        public MeetupSyncServices(IMeetupServices meetupServices,
            NetBairesContext context,
            ILogger<SyncServices> logger)
        {
            _meetupServices = meetupServices;
            _context = context;
            _logger = logger;
        }
        public async Task ProcessAttendees(Event eventToSync)
        {
            if (eventToSync.Platform != EventPlatform.Meetup)
                return;
            var meetupAttendees = await _meetupServices.GetAttendees(int.Parse(eventToSync.EventId));
            var meetupAttendeesIds = meetupAttendees.Select(s => s.Member.Id);
            var attendeesToEach = await _context.Attendances.Include(x => x.Member).Where(x => meetupAttendeesIds.Contains(x.Member.MeetupId)).ToListAsync();
            foreach (var attende in meetupAttendees)
            {
                var currentMember = attendeesToEach?.FirstOrDefault(x => x.Member.MeetupId == attende.Member.Id);
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
                            currentMember = new Attendance(newMember, eventToSync, true);
                        else
                            currentMember = new Attendance(newMember, eventToSync,false);
                        await _context.Attendances.AddAsync(currentMember);
                    }
                }
                else
                {
                    if ((attende.Status != null
                         &&
                         attende.Status == "attended"))
                        currentMember.Attend();
                    else if ((attende.Status != null
                              &&
                              attende.Status == "absent"))
                        currentMember.NoAttend();
                }

            }
            await _context.SaveChangesAsync();
        }
    }
}