using System.Threading.Tasks;

namespace NetBaires.Api.Services.Sync
{
    public interface ISyncServices
    {
        Task SyncEvent(int internalEventId);
    }
}