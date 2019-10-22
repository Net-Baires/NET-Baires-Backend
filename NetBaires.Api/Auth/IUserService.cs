using System.Threading.Tasks;

namespace NetBaires.Api.Auth
{
    public interface IUserService
    {
        Task<AuthenticateUser> AuthenticateOrCreate(string email);
        ValidateToken Validate(string tokenToValidate);
    }
}