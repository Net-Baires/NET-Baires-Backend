using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Services.EventBrite;
using NetBaires.Data;
using NetBaires.Data.Entities;
using Member = NetBaires.Data.Entities.Member;

namespace NetBaires.Api.Services.Sync
{
    public class EventBriteSyncServices : IExternalsSyncServices
    {
        private readonly IEventBriteServices _eventBriteServices;
        private readonly NetBairesContext _context;
        private readonly ILogger<SyncServices> _logger;

        public EventBriteSyncServices(IEventBriteServices eventBriteServices,
         NetBairesContext context,
         ILogger<SyncServices> logger)
        {
            _eventBriteServices = eventBriteServices;
            _context = context;
            _logger = logger;
        }
        public async Task ProcessAttendees(Event eventToSync)
        {
            if (eventToSync.Platform != EventPlatform.EventBrite)
                return;
            var meetupAttendees = await _eventBriteServices.GetAttendees(eventToSync.EventId);
            foreach (var attendees in meetupAttendees)
            {
                var attendace = eventToSync.Attendees.FirstOrDefault(x => x.Member.Email == attendees.Profile.Email);
                if (attendace == null)
                {
                    var member = await _context.Members.FirstOrDefaultAsync(x => x.Email.ToUpper() == attendees.Profile.Email.ToUpper());
                    if (member == null)
                        member = new Member
                        {
                            FirstName = attendees.Profile.FirstName,
                            LastName = attendees.Profile.LastName,
                            Email = attendees.Profile.Email,
                        };
                    if ((attendees.CheckIn))
                        attendace = new Attendance(member, eventToSync, true, AttendanceRegisterType.ExternalPage);
                    else
                        attendace = new Attendance(member, eventToSync, false, AttendanceRegisterType.ExternalPage);
                    await _context.Attendances.AddAsync(attendace);
                }
                else
                {
                    if ((attendees.CheckIn))
                        attendace.Attend();
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}