using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Auth.AuthEventBrite;
using NetBaires.Data.Entities;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Services
{
    public class UserServiceShould : IntegrationTestsBase
    {

        public UserServiceShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task Login_Meetup_With_The_Existed_Member()
        {
            var email = "test@test.com";
            var newMember = new Member
            {
                MeetupId = 1231243123
            };
            Context.Members.Add(newMember);
            Context.SaveChanges();
            var result = await UserService.AuthenticateOrCreate(email, newMember.MeetupId);
            RefreshContext();
            var members = await Context.Members.Where(x => x.MeetupId == newMember.MeetupId
                                                           ||
                                                           x.Email == email).ToListAsync();
            members.Count.Should().Be(1);
            members.First().MeetupId.Should().Be(newMember.MeetupId);
            members.First().Email.Should().Be(email);
        }
        [Fact]
        public async Task Login_Meetup_With_User_That_Is_Broken_In_Two()
        {
            var email = "test@test.com";
            var newMemberWithMeetupId = new Member
            {
                MeetupId = 1231243123
            };
            var newMemberWithEmailWithoutMeetupId = new Member
            {
                Email = email
            };
            Context.Members.Add(newMemberWithMeetupId);
            Context.Members.Add(newMemberWithEmailWithoutMeetupId);
            Context.SaveChanges();
            var result = await UserService.AuthenticateOrCreate(email, newMemberWithMeetupId.MeetupId);
            RefreshContext();
            var members = await Context.Members.Where(x => x.MeetupId == newMemberWithMeetupId.MeetupId
                                                           &&
                                                           x.Email == email).ToListAsync();
            members.Count.Should().Be(1);
            members.First().MeetupId.Should().Be(newMemberWithMeetupId.MeetupId);
            members.First().Email.Should().Be(email);
        }
        [Fact]
        public async Task Login_Eventbrite_With_The_Existed_Member()
        {
            var email = "test@test.com";
            var newMember = new Member
            {
                Email = email
            };
            Context.Members.Add(newMember);
            Context.SaveChanges();
            var meEventbrite = new EventBriteMe
            {
                Emails = new Email[2] { new Email { EmailEmail = email }, new Email() },
                FirstName = "First",
                LastName = "LastName",
                Id = "123123123123"
            };
            var result = await UserService.AuthenticateOrCreateEventbrite(meEventbrite);
            RefreshContext();
            var members = await Context.Members.Where(x => x.EventbriteId == meEventbrite.Id
                                                           ||
                                                           x.Email == email).ToListAsync();
            members.Count.Should().Be(1);
            members.First().EventbriteId.Should().Be(newMember.EventbriteId);
            members.First().Email.Should().Be(email);
            members.First().FirstName.Should().Be(meEventbrite.FirstName);
            members.First().LastName.Should().Be(meEventbrite.LastName);
        }

        [Fact]
        public async Task Login_Eventbrite_With_The_new_Member()
        {
            var email = "test@test.com";
            var meEventbrite = new EventBriteMe
            {
                Emails = new Email[2] { new Email { EmailEmail = email }, new Email() },
                FirstName = "First",
                LastName = "LastName",
                Id = "123123123123"
            };
            var result = await UserService.AuthenticateOrCreateEventbrite(meEventbrite);
            RefreshContext();
            var members = await Context.Members.Where(x => x.EventbriteId == meEventbrite.Id
                                                           ||
                                                           x.Email == email).ToListAsync();
            members.Count.Should().Be(1);
            members.First().EventbriteId.Should().Be(meEventbrite.Id);
            members.First().Email.Should().Be(email);
            members.First().FirstName.Should().Be(meEventbrite.FirstName);
            members.First().LastName.Should().Be(meEventbrite.LastName);
        }
        [Fact]
        public async Task Login_Eventbrite_With_The_Existed_All_Data_Member()
        {
            var email = "test@test.com";
            var eventbriteId = "123123123123";
            var newMember = new Member
            {
                Email = email,
                EventbriteId = eventbriteId
            };
            Context.Members.Add(newMember);
            Context.SaveChanges();
            var meEventbrite = new EventBriteMe
            {
                Emails = new Email[2] { new Email { EmailEmail = email }, new Email() },
                FirstName = "First",
                LastName = "LastName",
                Id = eventbriteId
            };
            var result = await UserService.AuthenticateOrCreateEventbrite(meEventbrite);
            RefreshContext();
            var members = await Context.Members.Where(x => x.EventbriteId == meEventbrite.Id
                                                           ||
                                                           x.Email == email).ToListAsync();
            members.Count.Should().Be(1);
            members.First().EventbriteId.Should().Be(newMember.EventbriteId);
            members.First().Email.Should().Be(email);
            members.First().FirstName.Should().BeNullOrEmpty();
            members.First().LastName.Should().BeNullOrEmpty();
        }
    }
}