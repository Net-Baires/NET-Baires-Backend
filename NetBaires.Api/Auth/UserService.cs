using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth.CustomsClaims;
using NetBaires.Api.Auth.Tokens;
using NetBaires.Api.Features.Auth.AuthEventBrite;
using NetBaires.Api.Models.ServicesResponse;
using NetBaires.Api.Options;
using NetBaires.Data;
using NetBaires.Data.Entities;
using Newtonsoft.Json;

namespace NetBaires.Api.Auth
{
    public static class StringExtension
    {
        public static string LowercaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            char[] a = s.ToCharArray();
            a[0] = char.ToLower(a[0]);

            return new string(a);
        }

    }
    public class UserService : IUserService
    {
        private readonly NetBairesContext _context;
        private readonly IMapper mapper;

        private readonly AppSettings _appSettings;
        private HttpClient _client;

        public UserService(NetBairesContext context,
        IMapper mapper,
        IHttpClientFactory httpClientFactory,
         IHttpContextAccessor httpContextAccessor,
        IOptions<AppSettings> appSettings)
        {
            _context = context;
            this.mapper = mapper;
            _appSettings = appSettings.Value;
            _client = httpClientFactory.CreateClient();
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
            return new AuthenticateUser(TokenService.Generate(_appSettings.Secret, new List<CustomClaim>
            {
                new CustomClaim(ClaimTypes.Name, id.ToString()),
                new CustomClaim(ClaimTypes.Email, email),
                new CustomClaim(ClaimTypes.Role, rol.ToString())
            }, DateTime.UtcNow.AddDays(30)));
        }
        public async Task<AuthenticateUser> AuthenticateOrCreate(string email, long meetupId)
        {
            //FIX : Fixea el error productivo donde usuarios que ya estaban importados con meetupID y sin email
            //cuando se loguearon con meetup por primera vez les creo un usuario con el mail pero con MeerupID 0 (quedando separados los mismos usuarios
            //separados en 2, uno con email otro con MeetupID)
            var users = await _context.Members.Where(x => x.Email.ToUpper() == email.ToUpper()
                                         ||
                                         x.MeetupId == meetupId).ToListAsync();
            var user = new Member();
            if (users.Count >= 2)
            {
                user = users.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Email));
            }
            else
            {
                user = users.FirstOrDefault();
            }
            if (user == null)
            {
                user = new Member
                {
                    Email = email,
                    MeetupId = meetupId,
                    Role = UserRole.Member
                };
                await _context.Members.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            else if (string.IsNullOrWhiteSpace(user.Email) || user.MeetupId == null || user.MeetupId == 0)
            {
                user.Email = email;
                user.MeetupId = meetupId;
                await _context.SaveChangesAsync();
            }

            return new AuthenticateUser(TokenService.Generate(_appSettings.Secret, new List<CustomClaim>
            {
                new CustomClaim(EnumClaims.UserId.ToString().LowercaseFirst(), user.Id.ToString()),
                new CustomClaim(EnumClaims.Email.ToString().LowercaseFirst(), user.Email),
                new CustomClaim(EnumClaims.Role.ToString().LowercaseFirst(), user.Role.ToString())
            }, DateTime.UtcNow.AddDays(30)));
        }

        public async Task<AuthenticateUser> AuthenticateOrCreateEventbrite(EventBriteMe me)
        {
            var user = await _context.Members.Where(x => x.Email.ToUpper() == me.Emails.First().EmailEmail.ToUpper()
                                                          ||
                                                          x.EventbriteId == me.Id).FirstOrDefaultAsync();
         
            if (user == null)
            {
                user = new Member
                {
                    Email = me.Emails.First().EmailEmail,
                    FirstName =  me.FirstName,
                    LastName = me.LastName,
                    EventbriteId= me.Id,
                    Role = UserRole.Member
                };
                await _context.Members.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            else if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.EventbriteId))
            {
                user.Email = me.Emails.First().EmailEmail;
                user.EventbriteId = me.Id;
                user.FirstName = me.FirstName;
                user.LastName = me.LastName;
                await _context.SaveChangesAsync();
            }

            return new AuthenticateUser(TokenService.Generate(_appSettings.Secret, new List<CustomClaim>
            {
                new CustomClaim(EnumClaims.UserId.ToString().LowercaseFirst(), user.Id.ToString()),
                new CustomClaim(EnumClaims.Email.ToString().LowercaseFirst(), user.Email),
                new CustomClaim(EnumClaims.Role.ToString().LowercaseFirst(), user.Role.ToString())
            }, DateTime.UtcNow.AddDays(30)));
        }

        public LoginToken Validate(string tokenToValidate)
        {
            return TokenService.Validate<LoginToken>(_appSettings.Secret, tokenToValidate);
        }

        
    
}
}