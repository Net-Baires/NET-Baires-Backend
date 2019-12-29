using System.Linq;
using Microsoft.AspNetCore.Http;
using NetBaires.Api.Auth.CustomsClaims;
using NetBaires.Data;

namespace NetBaires.Api.Auth
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsLoggued => _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        private CurrentUserDto _user;
        public CurrentUserDto User
        {
            get
            {
                if (_user == null)
                {
                    var email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Contains(EnumClaims.Email.ToString().LowercaseFirst())).Value;
                    var id = int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == EnumClaims.UserId.ToString().LowercaseFirst()).Value);
                    var rol = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Contains(EnumClaims.Role.ToString().LowercaseFirst())).Value;
                    _user = new CurrentUserDto(email, id, EnumExtensions.ParseEnum<UserRole>(rol));
                }
                return _user;
            }
        }

    }
}