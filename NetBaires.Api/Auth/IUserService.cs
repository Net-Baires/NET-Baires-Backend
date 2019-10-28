using System.Threading.Tasks;

namespace NetBaires.Api.Auth
{
    public interface IUserService
    {
        Task<AuthenticateUser> AuthenticateOrCreate(string email);
        Task<AuthenticateUser> AuthenticateOrCreate(string email, int meetupId);
        ValidateToken Validate(string tokenToValidate);
    }
}