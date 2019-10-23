using System.Collections.Generic;
using System.Threading.Tasks;
using NetBaires.Api.Services.Meetup.Models;

namespace NetBaires.Api.Services.Meetup
{
    public interface IMeetupServices
    {
        Task<List<MeetupEventDetail>> GetAllEvents();
        Task<List<AttendanceResponse>> GetAttendees(int eventId);
    }
}