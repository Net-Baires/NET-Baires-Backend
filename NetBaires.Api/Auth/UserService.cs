using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetBaires.Data;

namespace NetBaires.Api.Auth
{
    public class UserService : IUserService
    {
        private readonly NetBairesContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AppSettings _appSettings;

        public UserService(NetBairesContext context, IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.Value;
        }

        public async Task<AuthenticateUser> AuthenticateOrCreate(string email)
        {
            var user = _context.Members.SingleOrDefault(x => x.Email.ToUpper() == email.ToUpper());

            // return null if user not found
            if (user == null)
            {
                user = new Member
                {
                    Email = email,
                    Role = UserRole.Member
                };
                await _context.Members.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            return new AuthenticateUser(TokenService.Generate(_appSettings.Secret, new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }, DateTime.UtcNow.AddDays(30)));
        }
        public ValidateToken Validate(string tokenToValidate)
        {
            return TokenService.Validate(tokenToValidate, _appSettings.Secret);
        }
    }
}