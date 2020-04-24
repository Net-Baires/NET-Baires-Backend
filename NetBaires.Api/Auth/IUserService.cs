using System.Threading.Tasks;
using NetBaires.Api.Features.Auth.AuthEventBrite;

namespace NetBaires.Api.Auth
{
    public interface IUserService
    {
        Task<AuthenticateUser> AuthenticateOrCreate(string email, long meetupId);
        Task<AuthenticateUser> AuthenticateOrCreateEventbrite(EventBriteMe me);
        LoginToken Validate(string tokenToValidate);
    }
}