using FluentAssertions;
using NetBaires.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NetBaires.Api.ViewModels;
using NetBaires.Host;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Organizers
{
    public class GetOrganizersShould : IntegrationTestsBase
    {
        public GetOrganizersShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_List_Of_organizers()
        {
            var member = new Member
            {
                Email = "Test@test.com",
                Github = "Github Test",
                Organized = true
            };
            var member2 = new Member
            {
                Email = "Test2@test.com",
                Github = "Github Test",
                Organized = true
            };
            var member3 = new Member
            {
                Email = "Test3@test.com",
                Github = "Github Test",
                Organized = false
            };
            await Context.Members.AddAsync(member);
            await Context.Members.AddAsync(member2);
            await Context.Members.AddAsync(member3);
            await Context.SaveChangesAsync();

            var response = await HttpClient.GetAsync($"/organizers");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var memberResponse = await response.Content.ReadAsAsync<List<MemberDetailViewModel>>();
            memberResponse.Count.Should().Be(2);
            memberResponse.First().Email.Should().Be(member.Email);
            memberResponse[1].Email.Should().Be(member2.Email);
        }

      
    }
}
