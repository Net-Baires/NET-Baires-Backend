using FluentAssertions;
using NetBaires.Data;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NetBaires.Api.Features.Members.AddMember;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Members
{
    public class AddMemberShould : IntegrationTestsBase
    {
        public AddMemberShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Add_New_Member()
        {
            var newMember = new AddMemberCommand
            {
                Email = "test@test.com",
                FirstName = "First Name",
                LastName = "LastName",
                MeetupId = 12312312312
            };

            var response = await HttpClient.PostAsync($"/members/",
                new StringContent(JsonConvert.SerializeObject(newMember), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            RefreshContext();

            var member = Context.Members.First(x=> x.Email == "test@test.com");
            member.Email.Should().Be(newMember.Email);
            member.FirstName.Should().Be(newMember.FirstName);
            member.LastName.Should().Be(newMember.LastName);
            member.MeetupId.Should().Be(newMember.MeetupId);
        }

        [Fact]
        public async Task Not_Add_Member_With_Same_Email()
        {
            Context.Members.Add(new Member
            {
                Email = "test@test.com"
            });
            Context.SaveChanges();
            var newMember = new AddMemberCommand
            {
                Email = "test@test.com"
            };

            var response = await HttpClient.PostAsync($"/members/",
                new StringContent(JsonConvert.SerializeObject(newMember), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Not_Add_Member_With_Same_MeetupId()
        {
            Context.Members.Add(new Member
            {
                MeetupId = 12312312312
            });
            Context.SaveChanges();

            var newMember = new AddMemberCommand
            {
                MeetupId = 12312312312
            };

            var response = await HttpClient.PostAsync($"/members/",
                new StringContent(JsonConvert.SerializeObject(newMember), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }
    }
}
