using FluentAssertions;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Members.SearchMember;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Members
{
    public class UpdateInformationShould : IntegrationTestsBase
    {
        public UpdateInformationShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Add_PushNotification()
        {
            var newMember = new UpdateInformationCommand
            {
                PushNotificationId = "PushNotificationId1"
            };

            var response = await HttpClient.PutAsync($"/members/information",
                new StringContent(JsonConvert.SerializeObject(newMember), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();

            var member = Context.Members.Include(x => x.PushNotifications).First(x => x.Email == "admin@admin.com");
            member.PushNotifications.First().PushNotificationId.Should().Be(newMember.PushNotificationId);
            member.PushNotifications.Count.Should().Be(1);
        }

        [Fact]
        public async Task Not_Add_PushNotification_Repeat()
        {
            var newMember = new UpdateInformationCommand
            {
                PushNotificationId = "PushNotificationId1"
            };

            await HttpClient.PutAsync($"/members/information",
                new StringContent(JsonConvert.SerializeObject(newMember), Encoding.UTF8, "application/json"));
            await HttpClient.PutAsync($"/members/information",
                new StringContent(JsonConvert.SerializeObject(newMember), Encoding.UTF8, "application/json"));

            RefreshContext();
            var member = Context.Members.Include(x => x.PushNotifications).First(x => x.Email == "admin@admin.com");
            member.PushNotifications.First().PushNotificationId.Should().Be(newMember.PushNotificationId);
            member.PushNotifications.Count.Should().Be(1);
        }
        [Fact]
        public async Task NAd_Other_PushNotification()
        {
            var newPushNotification = new UpdateInformationCommand
            {
                PushNotificationId = "PushNotificationId1"
            };

            await HttpClient.PutAsync($"/members/information",
                new StringContent(JsonConvert.SerializeObject(newPushNotification), Encoding.UTF8, "application/json"));

            var newPushNotificationSecond = new UpdateInformationCommand
            {
                PushNotificationId = "PushNotificationId2"
            };
            await HttpClient.PutAsync($"/members/information",
                new StringContent(JsonConvert.SerializeObject(newPushNotificationSecond), Encoding.UTF8, "application/json"));

            RefreshContext();
            var member = Context.Members.Include(x => x.PushNotifications).First(x => x.Email == "admin@admin.com");
            member.PushNotifications.First().PushNotificationId.Should().Be(newPushNotification.PushNotificationId);
            member.PushNotifications[1].PushNotificationId.Should().Be(newPushNotificationSecond.PushNotificationId);
            member.PushNotifications.Count.Should().Be(2);
        }
    }
}
