using System.Threading.Tasks;
using NetBaires.Api.Models.ServicesResponse.Badgr;

namespace NetBaires.Api.Services.BadGr
{
    public interface IBadGrServices
    {
        Task<BadgrResponse<BadgClass>> GetAllBadget();
        Task<BadgrResponse<BadgClass>> CreateAssertion(string badgeId, string email);
    }
}