using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Api.ViewModels;
using NetBaires.Data;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Badges
{
    public class GetMembersInBadgeShould : IntegrationTestsBase
    {
        public GetMembersInBadgeShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
        }

        [Fact]
        public async Task Return_Members()
        {
            var badge = new Badge
            {
                Members = new List<BadgeMember> {
                    new BadgeMember{
                        Member = new Member{
                            Email="first@email.com"
                        }
                    },
                     new BadgeMember{
                        Member = new Member{
                            Email="second@email.com"
                        }
                    }
                }
            };
            await Context.Badges.AddAsync(badge);

            await Context.SaveChangesAsync();
            var response = await HttpClient.GetAsync($"/badges/{badge.Id}/Members");

            var badges = await response.Content.ReadAsAsync<List<MemberDetailViewModel>>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            badges.Count.Should().Be(2);
            badges.First().Email.Should().Be("first@email.com");
            badges[1].Email.Should().Be("second@email.com");
        }
        [Fact]
        public async Task Return_Empty_Result()
        {
            var badge = new Badge
            {
                Members = new List<BadgeMember> {
                    new BadgeMember{
                        Member = new Member{
                            Email="first@email.com"
                        }
                    },
                     new BadgeMember{
                        Member = new Member{
                            Email="second@email.com"
                        }
                    }
                }
            };
            await Context.Badges.AddAsync(badge);

            await Context.SaveChangesAsync();
            var response = await HttpClient.GetAsync($"/badges/99/Members");

            var badges = await response.Content.ReadAsAsync<List<MemberDetailViewModel>>();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}