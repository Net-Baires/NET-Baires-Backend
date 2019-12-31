using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Data;

namespace NetBaires.Api.Services.Sync
{
    public class SyncServices : ISyncServices
    {
        private readonly NetBairesContext _context;
        private readonly IEnumerable<IExternalsSyncServices> _services;
        private readonly ILogger<SyncServices> _logger;

        public SyncServices(NetBairesContext context,
            IEnumerable<IExternalsSyncServices> services,
            ILogger<SyncServices> logger)
        {
            _context = context;
            _services = services;
            _logger = logger;
        }
        public async Task SyncEvent(int internalEventId)
        {
            var eventToSync = _context.Events
                .Include(x => x.Attendees)
                .ThenInclude(x => x.Member)
                .FirstOrDefault(x => x.Id == internalEventId);

            if (eventToSync != null)
                foreach (var item in _services)
                    await item.ProcessAttendees(eventToSync);
        }
    }
}