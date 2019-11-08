using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Services.EventBrite;
using NetBaires.Data;
using Member = NetBaires.Data.Member;

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
            this._eventBriteServices = eventBriteServices;
            this._context = context;
            this._logger = logger;
        }
        public async Task ProcessAttendees(Event eventToSync)
        {
            if (eventToSync.Platform != EventPlatform.EventBrite)
                return;
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
                        currentMember = new Attendance(newMember, eventToSync, true);
                    else
                        currentMember = new Attendance(newMember, eventToSync, false);
                    await _context.Attendances.AddAsync(currentMember);
                }
                else
                {
                    if ((attendees.CheckIn))
                        currentMember.Attend();
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}