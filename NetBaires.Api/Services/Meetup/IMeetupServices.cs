using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetBaires.Api.Services.Meetup
{
    public interface IMeetupServices
    {
        Task<List<MeetupEventDetail>> GetAllEvents();
    }
}