using FluentAssertions;
using NetBaires.Api.Features.Members.ViewModels;
using NetBaires.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.Members
{
    public class SearchMemberShould : IntegrationTestsBase
    {
        public SearchMemberShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_Two_Members()
        {
            await FillData();

            var response = await HttpClient.GetAsync($"/members/Mar");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var memberResponse = await response.Content.ReadAsAsync<List<MemberDetailViewModel>>();
            memberResponse.Count().Should().Be(2);
        }

        private async Task FillData()
        {
            var members = new List<Member> {
                                             new Member
                                                    {
                                                        Email = "Test@test.com",
                                                        FirstName="Juan Marcos",
                                                        LastName ="Last Name",
                                                        MeetupId=11111111
                                                    },
                                                        new Member
                                                    {
                                                        Email = "Test@test.com",
                                                        FirstName="Martin",
                                                        LastName ="Perez",
                                                        MeetupId=11111111
                                                    },
                                                                   new Member
                                                    {
                                                        Email = "Test@test.com",
                                                        FirstName="Esteban",
                                                        LastName ="Pere",
                                                        MeetupId=11111111
                                                    },
                                                                              new Member
                                                    {
                                                        Email = "Test@test.com",
                                                        FirstName="Francisco",
                                                        LastName ="Alonso",
                                                        MeetupId=11111111
                                                    }
                                             };
            await Context.Members.AddRangeAsync(members);
            await Context.SaveChangesAsync();
        }

    }
}
