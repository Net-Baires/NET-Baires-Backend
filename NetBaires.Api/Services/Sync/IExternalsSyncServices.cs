using System.Threading.Tasks;
using NetBaires.Data;

namespace NetBaires.Api.Services.Sync
{
    public interface IExternalsSyncServices
    {
        Task ProcessAttendees(Event eventToSync);
    }
}