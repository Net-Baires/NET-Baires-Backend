using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetBaires.Data;

namespace NetBaires.Api.Auth
{
    public class UserService : IUserService
    {
        private readonly NetBairesContext _context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AppSettings _appSettings;

        public UserService(NetBairesContext context,
        IMapper mapper,
         IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> appSettings)
        {
            _context = context;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.Value;
        }

        public async Task<AuthenticateUser> AuthenticateOrCreate(string email, int meetupId)
        {
            var userByEmail = _context.Members.Include(x => x.Events).SingleOrDefault(x => x.Email.ToUpper() == email.ToUpper());
            var userByMeetupId = _context.Members.SingleOrDefault(x => x.MeetupId == meetupId && x.Email.ToUpper() != email.ToUpper());
            var id = 0;
            UserRole rol = UserRole.Member;

            // return null if user not found
            if (userByEmail == null && userByMeetupId == null)
            {
                userByEmail = new Member
                {
                    Email = email,
                    Role = UserRole.Member,
                    MeetupId = meetupId
                };
                await _context.Members.AddAsync(userByEmail);
                await _context.SaveChangesAsync();
                id = userByEmail.Id;
                rol = UserRole.Member;
            }
            else
            {
                if (userByEmail == null && userByMeetupId != null)
                {

                    userByMeetupId.Email = email.ToUpper();
                    id = userByMeetupId.Id;
                    rol = userByMeetupId.Role;
                }
                else if (userByEmail != null && userByMeetupId == null)
                {
                    userByEmail.MeetupId = meetupId;
                    id = userByEmail.Id;
                    rol = userByEmail.Role;
                }
                else if (userByEmail != null && userByMeetupId != null)
                {
                    userByEmail.MeetupId = meetupId;
                    var attendances = await _context.Attendances.Where(x => x.MemberId == userByMeetupId.Id).ToListAsync();
                    _context.Entry(userByMeetupId).State = EntityState.Deleted;
                    await _context.SaveChangesAsync();

                    foreach (var attendance in attendances)
                    {
                        var newToInsert = mapper.Map(attendance, new Attendance());
                        newToInsert.Member = userByEmail;
                        newToInsert.MemberId = userByEmail.Id;
                        _context.Entry(newToInsert).State = EntityState.Added;

                        await _context.Attendances.AddAsync(newToInsert);
                    }
                    id = userByEmail.Id;
                    rol = userByEmail.Role;
                    await _context.SaveChangesAsync();
                }
            }
            return new AuthenticateUser(TokenService.Generate(_appSettings.Secret, new List<Claim>
            {
                new Claim(ClaimTypes.Name, id.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, rol.ToString())
            }, DateTime.UtcNow.AddDays(30)));
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