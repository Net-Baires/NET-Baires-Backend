using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
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

        public bool IsLoggued => _httpContextAccessor.HttpContext.User != null;
        public CurrentUserDto User
        {
            get
            {

                var email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var id = int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value);
                var rol = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
                return new CurrentUserDto(email, id, EnumExtensions.ParseEnum<UserRole>(rol));
            }
        }

    }
}