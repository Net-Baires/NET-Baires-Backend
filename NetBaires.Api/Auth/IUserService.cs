using System.Threading.Tasks;

namespace NetBaires.Api.Auth
{
    public interface IUserService
    {
        Task<AuthenticateUser> AuthenticateOrCreate(string email, long meetupId);
        LoginToken Validate(string tokenToValidate);
    }
}