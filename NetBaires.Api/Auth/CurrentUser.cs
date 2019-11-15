using System.Linq;
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

        public bool IsLoggued => _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        public CurrentUserDto User
        {
            get
            {

                var email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == EnumClaims.Email.ToString()).Value;
                var id = int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == EnumClaims.UserId.ToString()).Value);
                var rol = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == EnumClaims.Role.ToString()).Value;
                return new CurrentUserDto(email, id, EnumExtensions.ParseEnum<UserRole>(rol));
            }
        }

    }
}