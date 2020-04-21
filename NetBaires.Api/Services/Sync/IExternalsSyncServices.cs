using System.Threading.Tasks;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Services.Sync
{
    public interface IExternalsSyncServices
    {
        Task ProcessAttendees(Event eventToSync);
    }
}